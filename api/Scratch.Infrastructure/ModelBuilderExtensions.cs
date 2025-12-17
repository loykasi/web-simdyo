using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Scratch.Domain.Entities;

namespace Scratch.Infrastructure
{
    public static class ModelBuilderExtensions
    {
        public static void UseVietnameseTables(this ModelBuilder builder)
        {
            // USERS
            builder.Entity<User>(entity =>
            {
                entity.ToTable("NguoiDung");

                entity.Property(u => u.RefreshToken).HasColumnName("TokenLamMoi");
                entity.Property(u => u.RefreshTokenExpriresAtUTC).HasColumnName("TokenLamMoi_ThoiHan");
                entity.Property(u => u.CreatedAt).HasColumnName("NgayTao");
                entity.Property(u => u.UpdatedAt).HasColumnName("NgayCapNhat");
                entity.Property(u => u.UserName).HasColumnName("TenDangNhap");
                entity.Property(u => u.NormalizedUserName).HasColumnName("TenDangNhapChuanHoa");
                entity.Property(u => u.Email).HasColumnName("Email");
                entity.Property(u => u.NormalizedEmail).HasColumnName("EmailChuanHoa");
                entity.Property(u => u.EmailConfirmed).HasColumnName("XacNhanEmail");
                entity.Property(u => u.PasswordHash).HasColumnName("MatKhauHash");
                entity.Property(u => u.SecurityStamp).HasColumnName("MaBaoMat");
                entity.Property(u => u.ConcurrencyStamp).HasColumnName("MaDongBo");
                entity.Property(u => u.PhoneNumber).HasColumnName("SoDienThoai");
                entity.Property(u => u.PhoneNumberConfirmed).HasColumnName("XacNhanSoDienThoai");
                entity.Property(u => u.TwoFactorEnabled).HasColumnName("XacThucHaiLop");
                entity.Property(u => u.LockoutEnd).HasColumnName("ThoiGianKhoa");
                entity.Property(u => u.LockoutEnabled).HasColumnName("KhoaTaiKhoan");
                entity.Property(u => u.AccessFailedCount).HasColumnName("SoLanDangNhapThatBai");
            });

            // REFRESH TOKENS
            builder.Entity<RefreshToken>(entity =>
            {
                entity.ToTable("TokenLamMoi");

                entity.Property(rt => rt.RefreshTokenExpriresAtUTC).HasColumnName("TokenLamMoi_ThoiHan");
                entity.Property(rt => rt.UserId).HasColumnName("IdNguoiDung");
            });

            // ROLES
            builder.Entity<Role>(entity =>
            {
                entity.ToTable("VaiTro");

                entity.Property(r => r.Name).HasColumnName("Ten");
                entity.Property(r => r.NormalizedName).HasColumnName("TenChuanHoa");
                entity.Property(r => r.ConcurrencyStamp).HasColumnName("MaDongBo");
            });

            // USER ROLES
            builder.Entity<IdentityUserRole<Guid>>(entity =>
            {
                entity.ToTable("NguoiDung_VaiTro");

                entity.Property(ur => ur.UserId).HasColumnName("IdNguoiDung");
                entity.Property(ur => ur.RoleId).HasColumnName("IdVaiTro");
            });

            // ROLE CLAIMS
            builder.Entity<IdentityRoleClaim<Guid>>(entity =>
            {
                entity.ToTable("VaiTroClaim");

                entity.Property(rc => rc.RoleId).HasColumnName("IdVaiTro");
                entity.Property(rc => rc.ClaimType).HasColumnName("KieuClaim");
                entity.Property(rc => rc.ClaimValue).HasColumnName("GiaTriClaim");
            });

            // USER CLAIMS
            builder.Entity<IdentityUserClaim<Guid>>(entity =>
            {
                entity.ToTable("NguoiDungClaim");

                entity.Property(uc => uc.UserId).HasColumnName("IdNguoiDung");
                entity.Property(uc => uc.ClaimType).HasColumnName("KieuClaim");
                entity.Property(uc => uc.ClaimValue).HasColumnName("GiaTriClaim");
            });

            // USER LOGINS
            builder.Entity<IdentityUserLogin<Guid>>(entity =>
            {
                entity.ToTable("NguoiDungDangNhap");

                entity.Property(ul => ul.LoginProvider).HasColumnName("NhaCungCap");
                entity.Property(ul => ul.ProviderKey).HasColumnName("KhoaNhaCungCap");
                entity.Property(ul => ul.ProviderDisplayName).HasColumnName("TenNhaCungCap");
                entity.Property(ul => ul.UserId).HasColumnName("NguoiDungId");

                entity.HasKey(ul => new { ul.LoginProvider, ul.ProviderKey });
            });

            // USER TOKENS
            builder.Entity<IdentityUserToken<Guid>>(entity =>
            {
                entity.ToTable("NguoiDungToken");

                entity.Property(ut => ut.UserId).HasColumnName("IdNguoiDung");
                entity.Property(ut => ut.LoginProvider).HasColumnName("NhaCungCap");
                entity.Property(ut => ut.Name).HasColumnName("Ten");
                entity.Property(ut => ut.Value).HasColumnName("GiaTri");

                entity.HasKey(ut => new
                {
                    ut.UserId,
                    ut.LoginProvider,
                    ut.Name
                });
            });

            // PROJECT CATEGORIES
            builder.Entity<ProjectCategory>(entity =>
            {
                entity.ToTable("DanhMucDuAn");

                entity.Property(p => p.Name).HasColumnName("Ten");
                entity.Property(u => u.CreatedAt).HasColumnName("NgayTao");
                entity.Property(u => u.UpdatedAt).HasColumnName("NgayCapNhat");
            });

            // PROJECTS
            builder.Entity<Project>(entity =>
            {
                entity.ToTable("DuAn");

                entity.Property(p => p.Name).HasColumnName("Ten");
                entity.Property(p => p.Description).HasColumnName("MoTa");
                entity.Property(p => p.PublicId).HasColumnName("IdCongKhai");
                entity.Property(p => p.FileLink).HasColumnName("LinkFile");
                entity.Property(p => p.ThumbnailLink).HasColumnName("LinkAnh");
                entity.Property(p => p.CategoryId).HasColumnName("IdDanhMuc");
                entity.Property(p => p.UserId).HasColumnName("IdNguoiDung");
                entity.Property(p => p.LikeCount).HasColumnName("LuotThich");
                entity.Property(p => p.DeletedAt).HasColumnName("NgayXoa");
                entity.Property(u => u.CreatedAt).HasColumnName("NgayTao");
                entity.Property(u => u.UpdatedAt).HasColumnName("NgayCapNhat");
            });

            // PROJECT BANS
            builder.Entity<ProjectBan>(entity =>
            {
                entity.ToTable("CamDuAn");

                entity.Property(p => p.Reason).HasColumnName("Lydo");
                entity.Property(p => p.Description).HasColumnName("MoTa");
                entity.Property(p => p.ProjectId).HasColumnName("IdDuAn");
                entity.Property(p => p.ByUserId).HasColumnName("IdNguoiThucHien");
                entity.Property(p => p.IsActive).HasColumnName("HieuLuc");
                entity.Property(p => p.RevokedByUserId).HasColumnName("IdNguoiThuHoi");
                entity.Property(u => u.CreatedAt).HasColumnName("NgayTao");
                entity.Property(u => u.UpdatedAt).HasColumnName("NgayCapNhat");
            });

            // PROJECT LIKES
            builder.Entity<ProjectLike>(entity =>
            {
                entity.ToTable("LuotThichDuAn");

                entity.Property(p => p.ProjectId).HasColumnName("IdDuAn");
                entity.Property(p => p.UserId).HasColumnName("IdNguoiDung");
                entity.Property(u => u.CreatedAt).HasColumnName("NgayTao");
                entity.Property(u => u.UpdatedAt).HasColumnName("NgayCapNhat");
            });

            // PROJECT COMMENTS
            builder.Entity<ProjectComment>(entity =>
            {
                entity.ToTable("BinhLuanDuAn");

                entity.Property(p => p.Content).HasColumnName("NoiDung");
                entity.Property(p => p.ParentCommentId).HasColumnName("IdTraLoi");
                entity.Property(p => p.ProjectId).HasColumnName("IdDuAn");
                entity.Property(p => p.UserId).HasColumnName("IdNguoiDung");
                entity.Property(p => p.RepliedUserId).HasColumnName("IdNguoiTraLoi");
                entity.Property(u => u.CreatedAt).HasColumnName("NgayTao");
                entity.Property(u => u.UpdatedAt).HasColumnName("NgayCapNhat");
            });

            // PROJECT REPORTS
            builder.Entity<ProjectReport>(entity =>
            {
                entity.ToTable("BaoCaoDuAn");

                entity.Property(p => p.Reason).HasColumnName("Lydo");
                entity.Property(p => p.Description).HasColumnName("MoTa");
                entity.Property(p => p.ProjectId).HasColumnName("IdDuAn");
                entity.Property(p => p.ByUserId).HasColumnName("IdNguoiThucHien");
                entity.Property(u => u.CreatedAt).HasColumnName("NgayTao");
                entity.Property(u => u.UpdatedAt).HasColumnName("NgayCapNhat");
            });

            // USER BANS
            builder.Entity<UserBan>(entity =>
            {
                entity.ToTable("CamNguoiDung");

                entity.Property(p => p.Reason).HasColumnName("Lydo");
                entity.Property(p => p.Description).HasColumnName("MoTa");
                entity.Property(p => p.UserId).HasColumnName("IdNguoiDung");
                entity.Property(p => p.ByUserId).HasColumnName("IdNguoiThucHien");
                entity.Property(p => p.IsActive).HasColumnName("HieuLuc");
                entity.Property(p => p.RevokedByUserId).HasColumnName("IdNguoiThuHoi");
                entity.Property(u => u.CreatedAt).HasColumnName("NgayTao");
                entity.Property(u => u.UpdatedAt).HasColumnName("NgayCapNhat");
            });
        }
    }
}
