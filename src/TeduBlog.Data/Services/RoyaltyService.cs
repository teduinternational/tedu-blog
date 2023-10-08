using Dapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using TeduBlog.Core.Domain.Identity;
using TeduBlog.Core.Domain.Royalty;
using TeduBlog.Core.SeedWorks;
using TeduBlog.Core.Services;

namespace TeduBlog.Data.Services
{
    public class RoyaltyService : IRoyaltyService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        public RoyaltyService(UserManager<AppUser> userManager,
            IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }
        public async Task<List<RoyaltyReportByMonthDto>> GetRoyaltyReportByMonthAsync(Guid? userId, int fromMonth, int fromYear, int toMonth, int toYear)
        {
            using (var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                if (conn.State == ConnectionState.Closed) await conn.OpenAsync();
                var coreSql = @"select 
                                    datepart(month,p.DateCreated) as Month,
                                    datepart(year,p.DateCreated) as Year,
                                    sum(case when p.Status = 0 then 1 else 0 end) as NumberOfDraftPosts,
                                    sum(case when p.Status = 1 then 1 else 0 end) as NumberOfWaitingApprovalPosts,
                                    sum(case when p.Status = 2 then 1 else 0 end) as NumberOfRejectedPosts,
                                    sum(case when p.Status = 3 then 1 else 0 end) as NumberOfPublishPosts,
                                    sum(case when p.Status = 3 and p.IsPaid = 1 then 1 else 0 end) as NumberOfPaidPublishPosts,
                                    sum(case when p.Status = 3 and p.IsPaid = 0 then 1 else 0 end) as NumberOfUnpaidPublishPosts
                                from Posts p
                                group by 
                                    datepart(month,p.DateCreated),
                                    datepart(year,p.DateCreated),
	                                p.AuthorUserId
                                having 
                                    (@fromMonth = 0 or datepart(month,p.DateCreated) >= @fromMonth) 
                                    and (@fromYear = 0 or datepart(year,p.DateCreated) >= @fromYear)
                                    and (@fromYear = 0 or datepart(month,p.DateCreated) <= @toMonth)
                                    and (@toYear = 0 or datepart(year,p.DateCreated) <= @toYear)
                                    and (@userId is null or p.AuthorUserId = @userId)";

                var items = await conn.QueryAsync<RoyaltyReportByMonthDto>(coreSql, new
                {
                    fromMonth,
                    fromYear,
                    toMonth,
                    toYear,
                    userId
                }, null, 120, CommandType.Text);
                return items.ToList();
            }
        }

        public async Task<List<RoyaltyReportByUserDto>> GetRoyaltyReportByUserAsync(Guid? userId, int fromMonth, int fromYear, int toMonth, int toYear)
        {
            using (var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                if (conn.State == ConnectionState.Closed) await conn.OpenAsync();
                var coreSql = @"select 
                                    u.Id as UserId,
                                    u.UserName as UserName,
                                    sum(case when p.Status = 0 then 1 else 0 end) as NumberOfDraftPosts,
                                    sum(case when p.Status = 1 then 1 else 0 end) as NumberOfWaitingApprovalPosts,
                                    sum(case when p.Status = 2 then 1 else 0 end) as NumberOfRejectedPosts,
                                    sum(case when p.Status = 3 then 1 else 0 end) as NumberOfPublishPosts,
                                    sum(case when p.Status = 3 and p.IsPaid = 1 then 1 else 0 end) as NumberOfPaidPublishPosts,
                                    sum(case when p.Status = 3 and p.IsPaid = 0 then 1 else 0 end) as NumberOfUnpaidPublishPosts
                                    from Posts p join AppUsers u on p.AuthorUserId = u.Id
                                    group by 
                                    datepart(month,p.DateCreated),
                                    datepart(year,p.DateCreated),
                                    p.AuthorUserId,
                                    u.Id,
                                    u.UserName
                                    having 
                                    (@fromMonth = 0 or datepart(month,p.DateCreated) >= @fromMonth) 
                                    and (@fromYear = 0 or datepart(year,p.DateCreated) >= @fromYear)
                                    and (@fromYear = 0 or datepart(month,p.DateCreated) <= @toMonth)
                                    and (@toYear = 0 or datepart(year,p.DateCreated) <= @toYear)
                                    and (@userId is null or p.AuthorUserId = @userId)";

                var items = await conn.QueryAsync<RoyaltyReportByUserDto>(coreSql, new
                {
                    fromMonth,
                    fromYear,
                    toMonth,
                    toYear,
                    userId
                }, null, 120, CommandType.Text);
                return items.ToList();
            }
        }

        public async Task PayRoyaltyForUserAsync(Guid fromUserId, Guid toUserId)
        {
            var fromUser = await _userManager.FindByIdAsync(fromUserId.ToString());
            if (fromUser == null)
            {
                throw new Exception($"User {fromUserId} not found");
            }
            var toUser = await _userManager.FindByIdAsync(toUserId.ToString());
            if (toUser == null)
            {
                throw new Exception($"User {toUserId} not found");
            }
            var unpaidPublishPosts = await _unitOfWork.Posts.GetListUnpaidPublishPosts(toUserId);
            double totalRoyalty = 0;
            foreach (var post in unpaidPublishPosts)
            {
                post.IsPaid = true;
                post.PaidDate = DateTime.Now;
                post.RoyaltyAmount = toUser.RoyaltyAmountPerPost;
                totalRoyalty += toUser.RoyaltyAmountPerPost;
            }

            toUser.Balance += totalRoyalty;
            await _userManager.UpdateAsync(toUser);
            _unitOfWork.Transactions.Add(new Transaction()
            {
                FromUserId = fromUser.Id,
                FromUserName = fromUser.UserName,
                ToUserId = toUserId,
                ToUserName = toUser.UserName,
                Amount = totalRoyalty,
                TransactionType = TransactionType.RoyaltyPay,
                Note = $"{fromUser.UserName} thanh toán nhuận bút cho {toUser.UserName}"
            });
            await _unitOfWork.CompleteAsync();
        }
    }
}