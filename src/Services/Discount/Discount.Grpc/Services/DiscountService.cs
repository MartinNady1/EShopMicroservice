

using Discount.Grpc.Data;
using Discount.Grpc.Models;
using Discount.Grpc.Protos;
using Grpc.Core;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Discount.Grpc.Services
{
    public class DiscountService(DiscountContext _context, ILogger<DiscountService> _logger)
        : DiscountProtoService.DiscountProtoServiceBase
    {
        public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
        {
            var coupon = await _context.Coupons.FirstOrDefaultAsync(x => x.ProductName == request.ProductName);

            if (coupon == null)
            {
                return new CouponModel { Amount = 0, Description = "No Discount", ProductName = request.ProductName };
            }
            _logger.LogInformation("Discount is retrieved for product name : {productName} amount : {amount}", coupon.ProductName, coupon.Amount);
            var couponModel = coupon.Adapt<CouponModel>();
            return couponModel;

        }
        public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
        {
            var coupon = request.Coupon.Adapt<Coupon>();
            if (coupon == null)
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid request"));
            }
            _context.Coupons.Add(coupon);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Discount is successfully created. Product Name : {productName}", request.Coupon.ProductName);
            var couponModel = coupon.Adapt<CouponModel>();
            return couponModel;
        }
        public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
        {
            var coupon = request.Coupon.Adapt<Coupon>();
            if (coupon == null)
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid request"));
            }
            _context.Coupons.Update(coupon);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Discount is successfully updated. Product Name : {productName}", request.Coupon.ProductName);
            var couponModel = coupon.Adapt<CouponModel>();
            return couponModel;
        }
        public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
        {
            var coupon = await _context.Coupons.FirstOrDefaultAsync(x => x.ProductName == request.Coupon.ProductName);
            if (coupon == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, "Discount not found"));
            }
            _context.Coupons.Remove(coupon);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Discount is successfully deleted. Product Name : {productName}", request.Coupon.ProductName);
            return new DeleteDiscountResponse {IsDeleted = true };
        }
    }
}
