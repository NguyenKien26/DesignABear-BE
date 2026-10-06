using DesignABear.Repositories.Models;
using DesignABear.Repositories.UnitOfWork;
using DesignABear.Services.DTOs.Responses;
using DesignABear.Services.Exceptions;
using DesignABear.Services.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DesignABear.Services.Services.Implementations
{
    public class ReportService : IReportService
    {
        private const int StaffRoleId = 2;
        private const decimal StaffMonthlySalary = 5_000_000m;

        private readonly IUnitOfWork _uow;

        public ReportService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<RevenueReportResponse> GetRevenueReportAsync(DateTime startDate, DateTime endDate)
        {
            startDate = DateTime.SpecifyKind(startDate.Date, DateTimeKind.Utc);
            endDate = DateTime.SpecifyKind(endDate.Date.AddDays(1).AddTicks(-1), DateTimeKind.Utc);

            var capturedPayments = await _uow.Payments.GetQueryable()
                .AsNoTracking()
                .Where(p =>
                    p.PaidAt.HasValue &&
                    p.PaidAt.Value >= startDate &&
                    p.PaidAt.Value <= endDate &&
                    p.Status != null &&
                    p.Status.ToUpper() == PaymentStatusConstants.Captured)
                .Select(p => new
                {
                    p.OrderId,
                    p.Amount,
                    PaidAt = p.PaidAt!.Value
                })
                .ToListAsync();

            var paidOrderIds = capturedPayments
                .Select(x => x.OrderId)
                .Distinct()
                .ToList();

            var orderPaidDateMap = capturedPayments
                .GroupBy(x => x.OrderId)
                .ToDictionary(g => g.Key, g => g.Min(x => x.PaidAt).Date);

            var refunds = await _uow.OrderRefunds.GetQueryable()
                .AsNoTracking()
                .Where(r =>
                    r.UpdatedAt >= startDate &&
                    r.UpdatedAt <= endDate &&
                    (r.Status.ToUpper() == "APPROVED" || r.Status.ToUpper() == "COMPLETED"))
                .Select(r => new
                {
                    r.Amount,
                    r.UpdatedAt
                })
                .ToListAsync();

            var orderItems = paidOrderIds.Any()
                ? await _uow.OrderItems.GetQueryable()
                    .AsNoTracking()
                    .Where(oi => paidOrderIds.Contains(oi.OrderId))
                    .Include(oi => oi.Variant)
                    .Include(oi => oi.Build)
                        .ThenInclude(b => b!.BaseVariant)
                    .Include(oi => oi.Build)
                        .ThenInclude(b => b!.BuildComponents)
                            .ThenInclude(bc => bc.OptionVariant)
                    .Include(oi => oi.ProductionJobs)
                    .ToListAsync()
                : new List<OrderItem>();

            var daily = new Dictionary<DateTime, DailyRevenue>();

            foreach (var p in capturedPayments)
            {
                var key = p.PaidAt.Date;
                if (!daily.TryGetValue(key, out var line))
                {
                    line = new DailyRevenue { Date = key };
                    daily[key] = line;
                }

                line.Revenue += p.Amount;
            }

            foreach (var item in orderItems)
            {
                if (!orderPaidDateMap.TryGetValue(item.OrderId, out var paidDate))
                {
                    continue;
                }

                if (!daily.TryGetValue(paidDate, out var line))
                {
                    line = new DailyRevenue { Date = paidDate };
                    daily[paidDate] = line;
                }

                line.ProductCost += CalculateProductCost(item);
                line.EmployeeCost += CalculateEmployeeCost(item);
            }

            foreach (var r in refunds)
            {
                var key = r.UpdatedAt.Date;
                if (!daily.TryGetValue(key, out var line))
                {
                    line = new DailyRevenue { Date = key };
                    daily[key] = line;
                }

                line.Refund += r.Amount;
            }

            foreach (var line in daily.Values)
            {
                line.NetRevenue = line.Revenue - line.Refund;
                line.Profit = line.NetRevenue - line.ProductCost - line.EmployeeCost;
            }

            var totalRevenue = daily.Values.Sum(x => x.Revenue);
            var totalRefund = daily.Values.Sum(x => x.Refund);
            var totalProductCost = daily.Values.Sum(x => x.ProductCost);
            var totalEmployeeCost = daily.Values.Sum(x => x.EmployeeCost);
            var netRevenue = totalRevenue - totalRefund;
            var totalProfit = netRevenue - totalProductCost - totalEmployeeCost;

            return new RevenueReportResponse
            {
                TotalRevenue = totalRevenue,
                NetRevenue = netRevenue,
                TotalProductCost = totalProductCost,
                TotalEmployeeCost = totalEmployeeCost,
                TotalRefund = totalRefund,
                TotalProfit = totalProfit,
                ProfitMarginPercent = netRevenue > 0 ? decimal.Round((totalProfit / netRevenue) * 100m, 2) : 0m,
                TotalOrders = paidOrderIds.Count,
                DailyBreakdown = daily.Values.OrderBy(x => x.Date).ToList()
            };
        }

        public async Task<PayrollReportResponse> GetPayrollReportAsync(DateTime startDate, DateTime endDate)
        {
            startDate = DateTime.SpecifyKind(startDate.Date, DateTimeKind.Utc);
            endDate = DateTime.SpecifyKind(endDate.Date.AddDays(1).AddTicks(-1), DateTimeKind.Utc);

            var payrollRows = await _uow.ProductionJobs.GetQueryable()
                .AsNoTracking()
                .Where(j => j.Status.ToUpper() == ProductionStatus.Finished ||
                            j.Status.ToUpper() == ProductionStatus.Completed ||
                            j.EspStatus.ToUpper() == ProductionStatus.Completed ||
                            j.ShellStatus.ToUpper() == ProductionStatus.Completed ||
                            j.QCStatus.ToUpper() == ProductionStatus.Completed ||
                            j.Status.ToUpper() == ProductionStatus.Assigned ||
                            j.Status.ToUpper() == ProductionStatus.InProgress ||
                            j.EspStatus.ToUpper() == ProductionStatus.InProgress ||
                            j.ShellStatus.ToUpper() == ProductionStatus.InProgress ||
                            j.QCStatus.ToUpper() == ProductionStatus.InProgress)
                .Select(j => new
                {
                    j.JobId,
                    j.TechnicianId,
                    j.QCInspectorId,
                    CraftsmanCommission = j.CraftsmanCommission == 0 ? 40000m : j.CraftsmanCommission,
                    QCCommission = j.QCCommission == 0 ? 10000m : j.QCCommission,
                    WorkAt = j.QCFinishedAt ?? j.FinishedAt ?? j.UpdatedAt
                })
                .Where(x => x.WorkAt >= startDate && x.WorkAt <= endDate)
                .ToListAsync();

            var employeeIds = payrollRows
                .SelectMany(x => new[] { x.TechnicianId, x.QCInspectorId })
                .Where(x => x.HasValue)
                .Select(x => x!.Value)
                .Distinct()
                .ToList();

            var users = employeeIds.Any()
                ? await _uow.Users.GetQueryable()
                    .AsNoTracking()
                    .Where(u => employeeIds.Contains(u.UserId))
                    .Select(u => new { u.UserId, u.FullName, u.Email })
                    .ToDictionaryAsync(x => x.UserId, x => new UserLite
                    {
                        FullName = x.FullName,
                        Email = x.Email
                    })
                : new Dictionary<Guid, UserLite>();

            var staffUsers = await _uow.Users.GetQueryable()
                .AsNoTracking()
                .Where(u => u.RoleId == StaffRoleId && u.Status == UserStatus.Active)
                .Select(u => new { u.UserId, u.FullName, u.Email })
                .ToListAsync();

            var staffPayrollPerUser = CalculateStaffPayroll(startDate, endDate);
            var employeeMap = new Dictionary<Guid, PayrollEmployeeLine>();
            var dailyMap = new Dictionary<DateTime, DailyPayroll>();

            foreach (var row in payrollRows)
            {
                if (row.TechnicianId.HasValue)
                {
                    AddPayroll(employeeMap, users, row.TechnicianId.Value, row.CraftsmanCommission, 0m, 0m);
                }

                if (row.QCInspectorId.HasValue)
                {
                    AddPayroll(employeeMap, users, row.QCInspectorId.Value, 0m, row.QCCommission, 0m);
                }

                var day = row.WorkAt.Date;
                if (!dailyMap.TryGetValue(day, out var daily))
                {
                    daily = new DailyPayroll { Date = day };
                    dailyMap[day] = daily;
                }

                daily.FinishedJobs += 1;
                daily.CraftsmanPayroll += row.CraftsmanCommission;
                daily.QCPayroll += row.QCCommission;
            }

            foreach (var staff in staffUsers)
            {
                if (!employeeMap.TryGetValue(staff.UserId, out var line))
                {
                    line = new PayrollEmployeeLine
                    {
                        EmployeeId = staff.UserId,
                        EmployeeName = string.IsNullOrWhiteSpace(staff.FullName) ? "N/A" : staff.FullName,
                        Email = staff.Email
                    };
                    employeeMap[staff.UserId] = line;
                }

                line.StaffAmount += staffPayrollPerUser;
            }

            foreach (var emp in employeeMap.Values)
            {
                emp.TotalAmount = emp.CraftsmanAmount + emp.QCAmount + emp.StaffAmount;
            }

            for (var day = startDate.Date; day <= endDate.Date; day = day.AddDays(1))
            {
                if (!dailyMap.TryGetValue(day, out var daily))
                {
                    daily = new DailyPayroll { Date = day };
                    dailyMap[day] = daily;
                }

                if (staffUsers.Count > 0)
                {
                    daily.StaffPayroll += CalculateStaffDailyRate(day) * staffUsers.Count;
                }
            }

            foreach (var daily in dailyMap.Values)
            {
                daily.TotalPayroll = daily.CraftsmanPayroll + daily.QCPayroll + daily.StaffPayroll;
            }

            var employees = employeeMap.Values
                .OrderByDescending(x => x.TotalAmount)
                .ToList();

            return new PayrollReportResponse
            {
                StartDate = startDate,
                EndDate = endDate,
                TotalFinishedJobs = payrollRows.Count,
                TotalCraftsmanPayroll = employees.Sum(x => x.CraftsmanAmount),
                TotalQCPayroll = employees.Sum(x => x.QCAmount),
                TotalStaffPayroll = employees.Sum(x => x.StaffAmount),
                TotalPayroll = employees.Sum(x => x.TotalAmount),
                Employees = employees,
                DailyBreakdown = dailyMap.Values.OrderBy(x => x.Date).ToList()
            };
        }

        public async Task<PayrollUserResponse> GetUserPayrollAsync(Guid userId, DateTime startDate, DateTime endDate)
        {
            startDate = DateTime.SpecifyKind(startDate.Date, DateTimeKind.Utc);
            endDate = DateTime.SpecifyKind(endDate.Date.AddDays(1).AddTicks(-1), DateTimeKind.Utc);

            var user = await _uow.Users.GetQueryable()
                .AsNoTracking()
                .Where(u => u.UserId == userId)
                .Select(u => new { u.UserId, u.FullName, u.Email, u.RoleId })
                .FirstOrDefaultAsync();

            if (user == null) throw new NotFoundException("User", userId);

            var payrollRows = await _uow.ProductionJobs.GetQueryable()
                .AsNoTracking()
                .Where(j => j.Status.ToUpper() == ProductionStatus.Finished ||
                            j.Status.ToUpper() == ProductionStatus.Completed ||
                            j.EspStatus.ToUpper() == ProductionStatus.Completed ||
                            j.ShellStatus.ToUpper() == ProductionStatus.Completed ||
                            j.QCStatus.ToUpper() == ProductionStatus.Completed ||
                            j.Status.ToUpper() == ProductionStatus.Assigned ||
                            j.Status.ToUpper() == ProductionStatus.InProgress ||
                            j.EspStatus.ToUpper() == ProductionStatus.InProgress ||
                            j.ShellStatus.ToUpper() == ProductionStatus.InProgress ||
                            j.QCStatus.ToUpper() == ProductionStatus.InProgress)
                .Select(j => new
                {
                    j.TechnicianId,
                    j.QCInspectorId,
                    CraftsmanCommission = j.CraftsmanCommission == 0 ? 40000m : j.CraftsmanCommission,
                    QCCommission = j.QCCommission == 0 ? 10000m : j.QCCommission,
                    WorkAt = j.QCFinishedAt ?? j.FinishedAt ?? j.UpdatedAt
                })
                .Where(x =>
                    x.WorkAt >= startDate &&
                    x.WorkAt <= endDate &&
                    (x.TechnicianId == userId || x.QCInspectorId == userId))
                .ToListAsync();

            var craftsmanAmount = payrollRows
                .Where(x => x.TechnicianId == userId)
                .Sum(x => x.CraftsmanCommission);

            var qcAmount = payrollRows
                .Where(x => x.QCInspectorId == userId)
                .Sum(x => x.QCCommission);

            var staffAmount = user.RoleId == StaffRoleId
                ? CalculateStaffPayroll(startDate, endDate)
                : 0m;

            return new PayrollUserResponse
            {
                UserId = user.UserId,
                EmployeeName = string.IsNullOrWhiteSpace(user.FullName) ? "N/A" : user.FullName,
                Email = user.Email,
                RoleId = user.RoleId,
                StartDate = startDate,
                EndDate = endDate,
                JobsCount = payrollRows.Count,
                CraftsmanAmount = craftsmanAmount,
                QCAmount = qcAmount,
                StaffAmount = staffAmount,
                TotalAmount = craftsmanAmount + qcAmount + staffAmount
            };
        }

        private static decimal CalculateProductCost(OrderItem item)
        {
            var qty = item.Quantity > 0 ? item.Quantity : 1;

            if (item.Variant != null)
            {
                return item.Variant.BaseCost * qty;
            }

            if (item.Build != null)
            {
                var baseVariantCost = item.Build.BaseVariant?.BaseCost ?? 0m;

                var componentsUnitCost = item.Build.BuildComponents?.Sum(c =>
                {
                    var componentQty = c.Quantity > 0 ? c.Quantity : 1;
                    return (c.OptionVariant?.BaseCost ?? 0m) * componentQty;
                }) ?? 0m;

                var unitCost = baseVariantCost + componentsUnitCost;
                return unitCost * qty;
            }

            return 0m;
        }

        private static decimal CalculateEmployeeCost(OrderItem item)
        {
            if (item.ProductionJobs != null && item.ProductionJobs.Any())
            {
                return item.ProductionJobs.Sum(j => j.CraftsmanCommission + j.QCCommission);
            }

            var qty = item.Quantity > 0 ? item.Quantity : 1;
            var assemblyUnitCost = item.Variant?.AssemblyCost ?? item.Build?.BaseVariant?.AssemblyCost ?? 0m;
            return assemblyUnitCost * qty;
        }

        private static decimal CalculateStaffPayroll(DateTime startDate, DateTime endDate)
        {
            var total = 0m;
            for (var day = startDate.Date; day <= endDate.Date; day = day.AddDays(1))
            {
                total += CalculateStaffDailyRate(day);
            }

            return total;
        }

        private static decimal CalculateStaffDailyRate(DateTime day)
        {
            var daysInMonth = DateTime.DaysInMonth(day.Year, day.Month);
            return StaffMonthlySalary / daysInMonth;
        }

        private static void AddPayroll(
            IDictionary<Guid, PayrollEmployeeLine> map,
            IReadOnlyDictionary<Guid, UserLite> users,
            Guid userId,
            decimal craftsmanAmount,
            decimal qcAmount,
            decimal staffAmount)
        {
            if (!map.TryGetValue(userId, out var line))
            {
                users.TryGetValue(userId, out var user);
                line = new PayrollEmployeeLine
                {
                    EmployeeId = userId,
                    EmployeeName = string.IsNullOrWhiteSpace(user?.FullName) ? "N/A" : user!.FullName!,
                    Email = user?.Email
                };
                map[userId] = line;
            }

            line.JobsCount += 1;
            line.CraftsmanAmount += craftsmanAmount;
            line.QCAmount += qcAmount;
            line.StaffAmount += staffAmount;
            line.TotalAmount = line.CraftsmanAmount + line.QCAmount + line.StaffAmount;
        }

        private sealed class UserLite
        {
            public string? FullName { get; set; }
            public string? Email { get; set; }
        }
    }
}
