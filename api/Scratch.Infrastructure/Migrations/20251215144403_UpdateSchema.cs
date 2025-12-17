using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Scratch.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                table: "AspNetRoleClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                table: "AspNetUserClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                table: "AspNetUserLogins");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                table: "AspNetUserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                table: "AspNetUserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                table: "AspNetUserTokens");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectBans_AspNetUsers_ByUserId",
                table: "ProjectBans");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectBans_AspNetUsers_RevokedByUserId",
                table: "ProjectBans");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectBans_Projects_ProjectId",
                table: "ProjectBans");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectComments_AspNetUsers_RepliedUserId",
                table: "ProjectComments");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectComments_AspNetUsers_UserId",
                table: "ProjectComments");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectComments_ProjectComments_ParentCommentId",
                table: "ProjectComments");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectComments_Projects_ProjectId",
                table: "ProjectComments");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectLikes_AspNetUsers_UserId",
                table: "ProjectLikes");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectLikes_Projects_ProjectId",
                table: "ProjectLikes");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectReports_AspNetUsers_ByUserId",
                table: "ProjectReports");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectReports_Projects_ProjectId",
                table: "ProjectReports");

            migrationBuilder.DropForeignKey(
                name: "FK_Projects_AspNetUsers_UserId",
                table: "Projects");

            migrationBuilder.DropForeignKey(
                name: "FK_Projects_ProjectCategories_CategoryId",
                table: "Projects");

            migrationBuilder.DropForeignKey(
                name: "FK_UserBans_AspNetUsers_ByUserId",
                table: "UserBans");

            migrationBuilder.DropForeignKey(
                name: "FK_UserBans_AspNetUsers_RevokedByUserId",
                table: "UserBans");

            migrationBuilder.DropForeignKey(
                name: "FK_UserBans_AspNetUsers_UserId",
                table: "UserBans");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserBans",
                table: "UserBans");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Projects",
                table: "Projects");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProjectReports",
                table: "ProjectReports");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProjectLikes",
                table: "ProjectLikes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProjectComments",
                table: "ProjectComments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProjectCategories",
                table: "ProjectCategories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProjectBans",
                table: "ProjectBans");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetUserTokens",
                table: "AspNetUserTokens");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetUsers",
                table: "AspNetUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetUserRoles",
                table: "AspNetUserRoles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetUserLogins",
                table: "AspNetUserLogins");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetUserClaims",
                table: "AspNetUserClaims");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetRoles",
                table: "AspNetRoles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetRoleClaims",
                table: "AspNetRoleClaims");

            migrationBuilder.RenameTable(
                name: "UserBans",
                newName: "CamNguoiDung");

            migrationBuilder.RenameTable(
                name: "Projects",
                newName: "DuAn");

            migrationBuilder.RenameTable(
                name: "ProjectReports",
                newName: "BaoCaoDuAn");

            migrationBuilder.RenameTable(
                name: "ProjectLikes",
                newName: "LuotThichDuAn");

            migrationBuilder.RenameTable(
                name: "ProjectComments",
                newName: "BinhLuanDuAn");

            migrationBuilder.RenameTable(
                name: "ProjectCategories",
                newName: "DanhMucDuAn");

            migrationBuilder.RenameTable(
                name: "ProjectBans",
                newName: "CamDuAn");

            migrationBuilder.RenameTable(
                name: "AspNetUserTokens",
                newName: "NguoiDungToken");

            migrationBuilder.RenameTable(
                name: "AspNetUsers",
                newName: "NguoiDung");

            migrationBuilder.RenameTable(
                name: "AspNetUserRoles",
                newName: "NguoiDung_VaiTro");

            migrationBuilder.RenameTable(
                name: "AspNetUserLogins",
                newName: "NguoiDungDangNhap");

            migrationBuilder.RenameTable(
                name: "AspNetUserClaims",
                newName: "NguoiDungClaim");

            migrationBuilder.RenameTable(
                name: "AspNetRoles",
                newName: "VaiTro");

            migrationBuilder.RenameTable(
                name: "AspNetRoleClaims",
                newName: "VaiTroClaim");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "CamNguoiDung",
                newName: "IdNguoiDung");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "CamNguoiDung",
                newName: "NgayCapNhat");

            migrationBuilder.RenameColumn(
                name: "RevokedByUserId",
                table: "CamNguoiDung",
                newName: "IdNguoiThuHoi");

            migrationBuilder.RenameColumn(
                name: "Reason",
                table: "CamNguoiDung",
                newName: "Lydo");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "CamNguoiDung",
                newName: "HieuLuc");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "CamNguoiDung",
                newName: "MoTa");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "CamNguoiDung",
                newName: "NgayTao");

            migrationBuilder.RenameColumn(
                name: "ByUserId",
                table: "CamNguoiDung",
                newName: "IdNguoiThucHien");

            migrationBuilder.RenameIndex(
                name: "IX_UserBans_UserId",
                table: "CamNguoiDung",
                newName: "IX_CamNguoiDung_IdNguoiDung");

            migrationBuilder.RenameIndex(
                name: "IX_UserBans_RevokedByUserId",
                table: "CamNguoiDung",
                newName: "IX_CamNguoiDung_IdNguoiThuHoi");

            migrationBuilder.RenameIndex(
                name: "IX_UserBans_ByUserId",
                table: "CamNguoiDung",
                newName: "IX_CamNguoiDung_IdNguoiThucHien");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "DuAn",
                newName: "IdNguoiDung");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "DuAn",
                newName: "NgayCapNhat");

            migrationBuilder.RenameColumn(
                name: "ThumbnailLink",
                table: "DuAn",
                newName: "LinkAnh");

            migrationBuilder.RenameColumn(
                name: "PublicId",
                table: "DuAn",
                newName: "IdCongKhai");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "DuAn",
                newName: "Ten");

            migrationBuilder.RenameColumn(
                name: "LikeCount",
                table: "DuAn",
                newName: "LuotThich");

            migrationBuilder.RenameColumn(
                name: "FileLink",
                table: "DuAn",
                newName: "LinkFile");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "DuAn",
                newName: "MoTa");

            migrationBuilder.RenameColumn(
                name: "DeletedAt",
                table: "DuAn",
                newName: "NgayXoa");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "DuAn",
                newName: "NgayTao");

            migrationBuilder.RenameColumn(
                name: "CategoryId",
                table: "DuAn",
                newName: "IdDanhMuc");

            migrationBuilder.RenameIndex(
                name: "IX_Projects_UserId",
                table: "DuAn",
                newName: "IX_DuAn_IdNguoiDung");

            migrationBuilder.RenameIndex(
                name: "IX_Projects_DeletedAt",
                table: "DuAn",
                newName: "IX_DuAn_NgayXoa");

            migrationBuilder.RenameIndex(
                name: "IX_Projects_CategoryId",
                table: "DuAn",
                newName: "IX_DuAn_IdDanhMuc");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "BaoCaoDuAn",
                newName: "NgayCapNhat");

            migrationBuilder.RenameColumn(
                name: "Reason",
                table: "BaoCaoDuAn",
                newName: "Lydo");

            migrationBuilder.RenameColumn(
                name: "ProjectId",
                table: "BaoCaoDuAn",
                newName: "IdDuAn");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "BaoCaoDuAn",
                newName: "MoTa");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "BaoCaoDuAn",
                newName: "NgayTao");

            migrationBuilder.RenameColumn(
                name: "ByUserId",
                table: "BaoCaoDuAn",
                newName: "IdNguoiThucHien");

            migrationBuilder.RenameIndex(
                name: "IX_ProjectReports_ProjectId",
                table: "BaoCaoDuAn",
                newName: "IX_BaoCaoDuAn_IdDuAn");

            migrationBuilder.RenameIndex(
                name: "IX_ProjectReports_ByUserId",
                table: "BaoCaoDuAn",
                newName: "IX_BaoCaoDuAn_IdNguoiThucHien");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "LuotThichDuAn",
                newName: "IdNguoiDung");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "LuotThichDuAn",
                newName: "NgayCapNhat");

            migrationBuilder.RenameColumn(
                name: "ProjectId",
                table: "LuotThichDuAn",
                newName: "IdDuAn");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "LuotThichDuAn",
                newName: "NgayTao");

            migrationBuilder.RenameIndex(
                name: "IX_ProjectLikes_UserId",
                table: "LuotThichDuAn",
                newName: "IX_LuotThichDuAn_IdNguoiDung");

            migrationBuilder.RenameIndex(
                name: "IX_ProjectLikes_ProjectId",
                table: "LuotThichDuAn",
                newName: "IX_LuotThichDuAn_IdDuAn");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "BinhLuanDuAn",
                newName: "IdNguoiDung");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "BinhLuanDuAn",
                newName: "NgayCapNhat");

            migrationBuilder.RenameColumn(
                name: "RepliedUserId",
                table: "BinhLuanDuAn",
                newName: "IdNguoiTraLoi");

            migrationBuilder.RenameColumn(
                name: "ProjectId",
                table: "BinhLuanDuAn",
                newName: "IdDuAn");

            migrationBuilder.RenameColumn(
                name: "ParentCommentId",
                table: "BinhLuanDuAn",
                newName: "IdTraLoi");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "BinhLuanDuAn",
                newName: "NgayTao");

            migrationBuilder.RenameColumn(
                name: "Content",
                table: "BinhLuanDuAn",
                newName: "NoiDung");

            migrationBuilder.RenameIndex(
                name: "IX_ProjectComments_UserId",
                table: "BinhLuanDuAn",
                newName: "IX_BinhLuanDuAn_IdNguoiDung");

            migrationBuilder.RenameIndex(
                name: "IX_ProjectComments_RepliedUserId",
                table: "BinhLuanDuAn",
                newName: "IX_BinhLuanDuAn_IdNguoiTraLoi");

            migrationBuilder.RenameIndex(
                name: "IX_ProjectComments_ProjectId",
                table: "BinhLuanDuAn",
                newName: "IX_BinhLuanDuAn_IdDuAn");

            migrationBuilder.RenameIndex(
                name: "IX_ProjectComments_ParentCommentId",
                table: "BinhLuanDuAn",
                newName: "IX_BinhLuanDuAn_IdTraLoi");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "DanhMucDuAn",
                newName: "NgayCapNhat");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "DanhMucDuAn",
                newName: "Ten");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "DanhMucDuAn",
                newName: "NgayTao");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "CamDuAn",
                newName: "NgayCapNhat");

            migrationBuilder.RenameColumn(
                name: "RevokedByUserId",
                table: "CamDuAn",
                newName: "IdNguoiThuHoi");

            migrationBuilder.RenameColumn(
                name: "Reason",
                table: "CamDuAn",
                newName: "Lydo");

            migrationBuilder.RenameColumn(
                name: "ProjectId",
                table: "CamDuAn",
                newName: "IdDuAn");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "CamDuAn",
                newName: "HieuLuc");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "CamDuAn",
                newName: "MoTa");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "CamDuAn",
                newName: "NgayTao");

            migrationBuilder.RenameColumn(
                name: "ByUserId",
                table: "CamDuAn",
                newName: "IdNguoiThucHien");

            migrationBuilder.RenameIndex(
                name: "IX_ProjectBans_RevokedByUserId",
                table: "CamDuAn",
                newName: "IX_CamDuAn_IdNguoiThuHoi");

            migrationBuilder.RenameIndex(
                name: "IX_ProjectBans_ProjectId",
                table: "CamDuAn",
                newName: "IX_CamDuAn_IdDuAn");

            migrationBuilder.RenameIndex(
                name: "IX_ProjectBans_ByUserId",
                table: "CamDuAn",
                newName: "IX_CamDuAn_IdNguoiThucHien");

            migrationBuilder.RenameColumn(
                name: "Value",
                table: "NguoiDungToken",
                newName: "GiaTri");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "NguoiDungToken",
                newName: "Ten");

            migrationBuilder.RenameColumn(
                name: "LoginProvider",
                table: "NguoiDungToken",
                newName: "NhaCungCap");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "NguoiDungToken",
                newName: "IdNguoiDung");

            migrationBuilder.RenameColumn(
                name: "UserName",
                table: "NguoiDung",
                newName: "TenDangNhap");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "NguoiDung",
                newName: "NgayCapNhat");

            migrationBuilder.RenameColumn(
                name: "TwoFactorEnabled",
                table: "NguoiDung",
                newName: "XacThucHaiLop");

            migrationBuilder.RenameColumn(
                name: "SecurityStamp",
                table: "NguoiDung",
                newName: "MaBaoMat");

            migrationBuilder.RenameColumn(
                name: "RefreshTokenExpriresAtUTC",
                table: "NguoiDung",
                newName: "TokenLamMoi_ThoiHan");

            migrationBuilder.RenameColumn(
                name: "RefreshToken",
                table: "NguoiDung",
                newName: "TokenLamMoi");

            migrationBuilder.RenameColumn(
                name: "PhoneNumberConfirmed",
                table: "NguoiDung",
                newName: "XacNhanSoDienThoai");

            migrationBuilder.RenameColumn(
                name: "PhoneNumber",
                table: "NguoiDung",
                newName: "SoDienThoai");

            migrationBuilder.RenameColumn(
                name: "PasswordHash",
                table: "NguoiDung",
                newName: "MatKhauHash");

            migrationBuilder.RenameColumn(
                name: "NormalizedUserName",
                table: "NguoiDung",
                newName: "TenDangNhapChuanHoa");

            migrationBuilder.RenameColumn(
                name: "NormalizedEmail",
                table: "NguoiDung",
                newName: "EmailChuanHoa");

            migrationBuilder.RenameColumn(
                name: "LockoutEnd",
                table: "NguoiDung",
                newName: "ThoiGianKhoa");

            migrationBuilder.RenameColumn(
                name: "LockoutEnabled",
                table: "NguoiDung",
                newName: "KhoaTaiKhoan");

            migrationBuilder.RenameColumn(
                name: "EmailConfirmed",
                table: "NguoiDung",
                newName: "XacNhanEmail");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "NguoiDung",
                newName: "NgayTao");

            migrationBuilder.RenameColumn(
                name: "ConcurrencyStamp",
                table: "NguoiDung",
                newName: "MaDongBo");

            migrationBuilder.RenameColumn(
                name: "AccessFailedCount",
                table: "NguoiDung",
                newName: "SoLanDangNhapThatBai");

            migrationBuilder.RenameColumn(
                name: "RoleId",
                table: "NguoiDung_VaiTro",
                newName: "IdVaiTro");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "NguoiDung_VaiTro",
                newName: "IdNguoiDung");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "NguoiDung_VaiTro",
                newName: "IX_NguoiDung_VaiTro_IdVaiTro");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "NguoiDungDangNhap",
                newName: "NguoiDungId");

            migrationBuilder.RenameColumn(
                name: "ProviderDisplayName",
                table: "NguoiDungDangNhap",
                newName: "TenNhaCungCap");

            migrationBuilder.RenameColumn(
                name: "ProviderKey",
                table: "NguoiDungDangNhap",
                newName: "KhoaNhaCungCap");

            migrationBuilder.RenameColumn(
                name: "LoginProvider",
                table: "NguoiDungDangNhap",
                newName: "NhaCungCap");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "NguoiDungDangNhap",
                newName: "IX_NguoiDungDangNhap_NguoiDungId");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "NguoiDungClaim",
                newName: "IdNguoiDung");

            migrationBuilder.RenameColumn(
                name: "ClaimValue",
                table: "NguoiDungClaim",
                newName: "GiaTriClaim");

            migrationBuilder.RenameColumn(
                name: "ClaimType",
                table: "NguoiDungClaim",
                newName: "KieuClaim");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "NguoiDungClaim",
                newName: "IX_NguoiDungClaim_IdNguoiDung");

            migrationBuilder.RenameColumn(
                name: "NormalizedName",
                table: "VaiTro",
                newName: "TenChuanHoa");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "VaiTro",
                newName: "Ten");

            migrationBuilder.RenameColumn(
                name: "ConcurrencyStamp",
                table: "VaiTro",
                newName: "MaDongBo");

            migrationBuilder.RenameColumn(
                name: "RoleId",
                table: "VaiTroClaim",
                newName: "IdVaiTro");

            migrationBuilder.RenameColumn(
                name: "ClaimValue",
                table: "VaiTroClaim",
                newName: "GiaTriClaim");

            migrationBuilder.RenameColumn(
                name: "ClaimType",
                table: "VaiTroClaim",
                newName: "KieuClaim");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "VaiTroClaim",
                newName: "IX_VaiTroClaim_IdVaiTro");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CamNguoiDung",
                table: "CamNguoiDung",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DuAn",
                table: "DuAn",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BaoCaoDuAn",
                table: "BaoCaoDuAn",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LuotThichDuAn",
                table: "LuotThichDuAn",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BinhLuanDuAn",
                table: "BinhLuanDuAn",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DanhMucDuAn",
                table: "DanhMucDuAn",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CamDuAn",
                table: "CamDuAn",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_NguoiDungToken",
                table: "NguoiDungToken",
                columns: new[] { "IdNguoiDung", "NhaCungCap", "Ten" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_NguoiDung",
                table: "NguoiDung",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_NguoiDung_VaiTro",
                table: "NguoiDung_VaiTro",
                columns: new[] { "IdNguoiDung", "IdVaiTro" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_NguoiDungDangNhap",
                table: "NguoiDungDangNhap",
                columns: new[] { "NhaCungCap", "KhoaNhaCungCap" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_NguoiDungClaim",
                table: "NguoiDungClaim",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_VaiTro",
                table: "VaiTro",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_VaiTroClaim",
                table: "VaiTroClaim",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BaoCaoDuAn_DuAn_IdDuAn",
                table: "BaoCaoDuAn",
                column: "IdDuAn",
                principalTable: "DuAn",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BaoCaoDuAn_NguoiDung_IdNguoiThucHien",
                table: "BaoCaoDuAn",
                column: "IdNguoiThucHien",
                principalTable: "NguoiDung",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BinhLuanDuAn_BinhLuanDuAn_IdTraLoi",
                table: "BinhLuanDuAn",
                column: "IdTraLoi",
                principalTable: "BinhLuanDuAn",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BinhLuanDuAn_DuAn_IdDuAn",
                table: "BinhLuanDuAn",
                column: "IdDuAn",
                principalTable: "DuAn",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BinhLuanDuAn_NguoiDung_IdNguoiDung",
                table: "BinhLuanDuAn",
                column: "IdNguoiDung",
                principalTable: "NguoiDung",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BinhLuanDuAn_NguoiDung_IdNguoiTraLoi",
                table: "BinhLuanDuAn",
                column: "IdNguoiTraLoi",
                principalTable: "NguoiDung",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CamDuAn_DuAn_IdDuAn",
                table: "CamDuAn",
                column: "IdDuAn",
                principalTable: "DuAn",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CamDuAn_NguoiDung_IdNguoiThuHoi",
                table: "CamDuAn",
                column: "IdNguoiThuHoi",
                principalTable: "NguoiDung",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CamDuAn_NguoiDung_IdNguoiThucHien",
                table: "CamDuAn",
                column: "IdNguoiThucHien",
                principalTable: "NguoiDung",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CamNguoiDung_NguoiDung_IdNguoiDung",
                table: "CamNguoiDung",
                column: "IdNguoiDung",
                principalTable: "NguoiDung",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CamNguoiDung_NguoiDung_IdNguoiThuHoi",
                table: "CamNguoiDung",
                column: "IdNguoiThuHoi",
                principalTable: "NguoiDung",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CamNguoiDung_NguoiDung_IdNguoiThucHien",
                table: "CamNguoiDung",
                column: "IdNguoiThucHien",
                principalTable: "NguoiDung",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DuAn_DanhMucDuAn_IdDanhMuc",
                table: "DuAn",
                column: "IdDanhMuc",
                principalTable: "DanhMucDuAn",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DuAn_NguoiDung_IdNguoiDung",
                table: "DuAn",
                column: "IdNguoiDung",
                principalTable: "NguoiDung",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LuotThichDuAn_DuAn_IdDuAn",
                table: "LuotThichDuAn",
                column: "IdDuAn",
                principalTable: "DuAn",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LuotThichDuAn_NguoiDung_IdNguoiDung",
                table: "LuotThichDuAn",
                column: "IdNguoiDung",
                principalTable: "NguoiDung",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_NguoiDung_VaiTro_NguoiDung_IdNguoiDung",
                table: "NguoiDung_VaiTro",
                column: "IdNguoiDung",
                principalTable: "NguoiDung",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_NguoiDung_VaiTro_VaiTro_IdVaiTro",
                table: "NguoiDung_VaiTro",
                column: "IdVaiTro",
                principalTable: "VaiTro",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_NguoiDungClaim_NguoiDung_IdNguoiDung",
                table: "NguoiDungClaim",
                column: "IdNguoiDung",
                principalTable: "NguoiDung",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_NguoiDungDangNhap_NguoiDung_NguoiDungId",
                table: "NguoiDungDangNhap",
                column: "NguoiDungId",
                principalTable: "NguoiDung",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_NguoiDungToken_NguoiDung_IdNguoiDung",
                table: "NguoiDungToken",
                column: "IdNguoiDung",
                principalTable: "NguoiDung",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_VaiTroClaim_VaiTro_IdVaiTro",
                table: "VaiTroClaim",
                column: "IdVaiTro",
                principalTable: "VaiTro",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BaoCaoDuAn_DuAn_IdDuAn",
                table: "BaoCaoDuAn");

            migrationBuilder.DropForeignKey(
                name: "FK_BaoCaoDuAn_NguoiDung_IdNguoiThucHien",
                table: "BaoCaoDuAn");

            migrationBuilder.DropForeignKey(
                name: "FK_BinhLuanDuAn_BinhLuanDuAn_IdTraLoi",
                table: "BinhLuanDuAn");

            migrationBuilder.DropForeignKey(
                name: "FK_BinhLuanDuAn_DuAn_IdDuAn",
                table: "BinhLuanDuAn");

            migrationBuilder.DropForeignKey(
                name: "FK_BinhLuanDuAn_NguoiDung_IdNguoiDung",
                table: "BinhLuanDuAn");

            migrationBuilder.DropForeignKey(
                name: "FK_BinhLuanDuAn_NguoiDung_IdNguoiTraLoi",
                table: "BinhLuanDuAn");

            migrationBuilder.DropForeignKey(
                name: "FK_CamDuAn_DuAn_IdDuAn",
                table: "CamDuAn");

            migrationBuilder.DropForeignKey(
                name: "FK_CamDuAn_NguoiDung_IdNguoiThuHoi",
                table: "CamDuAn");

            migrationBuilder.DropForeignKey(
                name: "FK_CamDuAn_NguoiDung_IdNguoiThucHien",
                table: "CamDuAn");

            migrationBuilder.DropForeignKey(
                name: "FK_CamNguoiDung_NguoiDung_IdNguoiDung",
                table: "CamNguoiDung");

            migrationBuilder.DropForeignKey(
                name: "FK_CamNguoiDung_NguoiDung_IdNguoiThuHoi",
                table: "CamNguoiDung");

            migrationBuilder.DropForeignKey(
                name: "FK_CamNguoiDung_NguoiDung_IdNguoiThucHien",
                table: "CamNguoiDung");

            migrationBuilder.DropForeignKey(
                name: "FK_DuAn_DanhMucDuAn_IdDanhMuc",
                table: "DuAn");

            migrationBuilder.DropForeignKey(
                name: "FK_DuAn_NguoiDung_IdNguoiDung",
                table: "DuAn");

            migrationBuilder.DropForeignKey(
                name: "FK_LuotThichDuAn_DuAn_IdDuAn",
                table: "LuotThichDuAn");

            migrationBuilder.DropForeignKey(
                name: "FK_LuotThichDuAn_NguoiDung_IdNguoiDung",
                table: "LuotThichDuAn");

            migrationBuilder.DropForeignKey(
                name: "FK_NguoiDung_VaiTro_NguoiDung_IdNguoiDung",
                table: "NguoiDung_VaiTro");

            migrationBuilder.DropForeignKey(
                name: "FK_NguoiDung_VaiTro_VaiTro_IdVaiTro",
                table: "NguoiDung_VaiTro");

            migrationBuilder.DropForeignKey(
                name: "FK_NguoiDungClaim_NguoiDung_IdNguoiDung",
                table: "NguoiDungClaim");

            migrationBuilder.DropForeignKey(
                name: "FK_NguoiDungDangNhap_NguoiDung_NguoiDungId",
                table: "NguoiDungDangNhap");

            migrationBuilder.DropForeignKey(
                name: "FK_NguoiDungToken_NguoiDung_IdNguoiDung",
                table: "NguoiDungToken");

            migrationBuilder.DropForeignKey(
                name: "FK_VaiTroClaim_VaiTro_IdVaiTro",
                table: "VaiTroClaim");

            migrationBuilder.DropPrimaryKey(
                name: "PK_VaiTroClaim",
                table: "VaiTroClaim");

            migrationBuilder.DropPrimaryKey(
                name: "PK_VaiTro",
                table: "VaiTro");

            migrationBuilder.DropPrimaryKey(
                name: "PK_NguoiDungToken",
                table: "NguoiDungToken");

            migrationBuilder.DropPrimaryKey(
                name: "PK_NguoiDungDangNhap",
                table: "NguoiDungDangNhap");

            migrationBuilder.DropPrimaryKey(
                name: "PK_NguoiDungClaim",
                table: "NguoiDungClaim");

            migrationBuilder.DropPrimaryKey(
                name: "PK_NguoiDung_VaiTro",
                table: "NguoiDung_VaiTro");

            migrationBuilder.DropPrimaryKey(
                name: "PK_NguoiDung",
                table: "NguoiDung");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LuotThichDuAn",
                table: "LuotThichDuAn");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DuAn",
                table: "DuAn");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DanhMucDuAn",
                table: "DanhMucDuAn");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CamNguoiDung",
                table: "CamNguoiDung");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CamDuAn",
                table: "CamDuAn");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BinhLuanDuAn",
                table: "BinhLuanDuAn");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BaoCaoDuAn",
                table: "BaoCaoDuAn");

            migrationBuilder.RenameTable(
                name: "VaiTroClaim",
                newName: "AspNetRoleClaims");

            migrationBuilder.RenameTable(
                name: "VaiTro",
                newName: "AspNetRoles");

            migrationBuilder.RenameTable(
                name: "NguoiDungToken",
                newName: "AspNetUserTokens");

            migrationBuilder.RenameTable(
                name: "NguoiDungDangNhap",
                newName: "AspNetUserLogins");

            migrationBuilder.RenameTable(
                name: "NguoiDungClaim",
                newName: "AspNetUserClaims");

            migrationBuilder.RenameTable(
                name: "NguoiDung_VaiTro",
                newName: "AspNetUserRoles");

            migrationBuilder.RenameTable(
                name: "NguoiDung",
                newName: "AspNetUsers");

            migrationBuilder.RenameTable(
                name: "LuotThichDuAn",
                newName: "ProjectLikes");

            migrationBuilder.RenameTable(
                name: "DuAn",
                newName: "Projects");

            migrationBuilder.RenameTable(
                name: "DanhMucDuAn",
                newName: "ProjectCategories");

            migrationBuilder.RenameTable(
                name: "CamNguoiDung",
                newName: "UserBans");

            migrationBuilder.RenameTable(
                name: "CamDuAn",
                newName: "ProjectBans");

            migrationBuilder.RenameTable(
                name: "BinhLuanDuAn",
                newName: "ProjectComments");

            migrationBuilder.RenameTable(
                name: "BaoCaoDuAn",
                newName: "ProjectReports");

            migrationBuilder.RenameColumn(
                name: "KieuClaim",
                table: "AspNetRoleClaims",
                newName: "ClaimType");

            migrationBuilder.RenameColumn(
                name: "IdVaiTro",
                table: "AspNetRoleClaims",
                newName: "RoleId");

            migrationBuilder.RenameColumn(
                name: "GiaTriClaim",
                table: "AspNetRoleClaims",
                newName: "ClaimValue");

            migrationBuilder.RenameIndex(
                name: "IX_VaiTroClaim_IdVaiTro",
                table: "AspNetRoleClaims",
                newName: "IX_AspNetRoleClaims_RoleId");

            migrationBuilder.RenameColumn(
                name: "TenChuanHoa",
                table: "AspNetRoles",
                newName: "NormalizedName");

            migrationBuilder.RenameColumn(
                name: "Ten",
                table: "AspNetRoles",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "MaDongBo",
                table: "AspNetRoles",
                newName: "ConcurrencyStamp");

            migrationBuilder.RenameColumn(
                name: "GiaTri",
                table: "AspNetUserTokens",
                newName: "Value");

            migrationBuilder.RenameColumn(
                name: "Ten",
                table: "AspNetUserTokens",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "NhaCungCap",
                table: "AspNetUserTokens",
                newName: "LoginProvider");

            migrationBuilder.RenameColumn(
                name: "IdNguoiDung",
                table: "AspNetUserTokens",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "TenNhaCungCap",
                table: "AspNetUserLogins",
                newName: "ProviderDisplayName");

            migrationBuilder.RenameColumn(
                name: "NguoiDungId",
                table: "AspNetUserLogins",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "KhoaNhaCungCap",
                table: "AspNetUserLogins",
                newName: "ProviderKey");

            migrationBuilder.RenameColumn(
                name: "NhaCungCap",
                table: "AspNetUserLogins",
                newName: "LoginProvider");

            migrationBuilder.RenameIndex(
                name: "IX_NguoiDungDangNhap_NguoiDungId",
                table: "AspNetUserLogins",
                newName: "IX_AspNetUserLogins_UserId");

            migrationBuilder.RenameColumn(
                name: "KieuClaim",
                table: "AspNetUserClaims",
                newName: "ClaimType");

            migrationBuilder.RenameColumn(
                name: "IdNguoiDung",
                table: "AspNetUserClaims",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "GiaTriClaim",
                table: "AspNetUserClaims",
                newName: "ClaimValue");

            migrationBuilder.RenameIndex(
                name: "IX_NguoiDungClaim_IdNguoiDung",
                table: "AspNetUserClaims",
                newName: "IX_AspNetUserClaims_UserId");

            migrationBuilder.RenameColumn(
                name: "IdVaiTro",
                table: "AspNetUserRoles",
                newName: "RoleId");

            migrationBuilder.RenameColumn(
                name: "IdNguoiDung",
                table: "AspNetUserRoles",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_NguoiDung_VaiTro_IdVaiTro",
                table: "AspNetUserRoles",
                newName: "IX_AspNetUserRoles_RoleId");

            migrationBuilder.RenameColumn(
                name: "XacThucHaiLop",
                table: "AspNetUsers",
                newName: "TwoFactorEnabled");

            migrationBuilder.RenameColumn(
                name: "XacNhanSoDienThoai",
                table: "AspNetUsers",
                newName: "PhoneNumberConfirmed");

            migrationBuilder.RenameColumn(
                name: "XacNhanEmail",
                table: "AspNetUsers",
                newName: "EmailConfirmed");

            migrationBuilder.RenameColumn(
                name: "TokenLamMoi_ThoiHan",
                table: "AspNetUsers",
                newName: "RefreshTokenExpriresAtUTC");

            migrationBuilder.RenameColumn(
                name: "TokenLamMoi",
                table: "AspNetUsers",
                newName: "RefreshToken");

            migrationBuilder.RenameColumn(
                name: "ThoiGianKhoa",
                table: "AspNetUsers",
                newName: "LockoutEnd");

            migrationBuilder.RenameColumn(
                name: "TenDangNhapChuanHoa",
                table: "AspNetUsers",
                newName: "NormalizedUserName");

            migrationBuilder.RenameColumn(
                name: "TenDangNhap",
                table: "AspNetUsers",
                newName: "UserName");

            migrationBuilder.RenameColumn(
                name: "SoLanDangNhapThatBai",
                table: "AspNetUsers",
                newName: "AccessFailedCount");

            migrationBuilder.RenameColumn(
                name: "SoDienThoai",
                table: "AspNetUsers",
                newName: "PhoneNumber");

            migrationBuilder.RenameColumn(
                name: "NgayTao",
                table: "AspNetUsers",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "NgayCapNhat",
                table: "AspNetUsers",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "MatKhauHash",
                table: "AspNetUsers",
                newName: "PasswordHash");

            migrationBuilder.RenameColumn(
                name: "MaDongBo",
                table: "AspNetUsers",
                newName: "ConcurrencyStamp");

            migrationBuilder.RenameColumn(
                name: "MaBaoMat",
                table: "AspNetUsers",
                newName: "SecurityStamp");

            migrationBuilder.RenameColumn(
                name: "KhoaTaiKhoan",
                table: "AspNetUsers",
                newName: "LockoutEnabled");

            migrationBuilder.RenameColumn(
                name: "EmailChuanHoa",
                table: "AspNetUsers",
                newName: "NormalizedEmail");

            migrationBuilder.RenameColumn(
                name: "NgayTao",
                table: "ProjectLikes",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "NgayCapNhat",
                table: "ProjectLikes",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "IdNguoiDung",
                table: "ProjectLikes",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "IdDuAn",
                table: "ProjectLikes",
                newName: "ProjectId");

            migrationBuilder.RenameIndex(
                name: "IX_LuotThichDuAn_IdNguoiDung",
                table: "ProjectLikes",
                newName: "IX_ProjectLikes_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_LuotThichDuAn_IdDuAn",
                table: "ProjectLikes",
                newName: "IX_ProjectLikes_ProjectId");

            migrationBuilder.RenameColumn(
                name: "Ten",
                table: "Projects",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "NgayXoa",
                table: "Projects",
                newName: "DeletedAt");

            migrationBuilder.RenameColumn(
                name: "NgayTao",
                table: "Projects",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "NgayCapNhat",
                table: "Projects",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "MoTa",
                table: "Projects",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "LuotThich",
                table: "Projects",
                newName: "LikeCount");

            migrationBuilder.RenameColumn(
                name: "LinkFile",
                table: "Projects",
                newName: "FileLink");

            migrationBuilder.RenameColumn(
                name: "LinkAnh",
                table: "Projects",
                newName: "ThumbnailLink");

            migrationBuilder.RenameColumn(
                name: "IdNguoiDung",
                table: "Projects",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "IdDanhMuc",
                table: "Projects",
                newName: "CategoryId");

            migrationBuilder.RenameColumn(
                name: "IdCongKhai",
                table: "Projects",
                newName: "PublicId");

            migrationBuilder.RenameIndex(
                name: "IX_DuAn_NgayXoa",
                table: "Projects",
                newName: "IX_Projects_DeletedAt");

            migrationBuilder.RenameIndex(
                name: "IX_DuAn_IdNguoiDung",
                table: "Projects",
                newName: "IX_Projects_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_DuAn_IdDanhMuc",
                table: "Projects",
                newName: "IX_Projects_CategoryId");

            migrationBuilder.RenameColumn(
                name: "Ten",
                table: "ProjectCategories",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "NgayTao",
                table: "ProjectCategories",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "NgayCapNhat",
                table: "ProjectCategories",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "NgayTao",
                table: "UserBans",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "NgayCapNhat",
                table: "UserBans",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "MoTa",
                table: "UserBans",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "Lydo",
                table: "UserBans",
                newName: "Reason");

            migrationBuilder.RenameColumn(
                name: "IdNguoiThucHien",
                table: "UserBans",
                newName: "ByUserId");

            migrationBuilder.RenameColumn(
                name: "IdNguoiThuHoi",
                table: "UserBans",
                newName: "RevokedByUserId");

            migrationBuilder.RenameColumn(
                name: "IdNguoiDung",
                table: "UserBans",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "HieuLuc",
                table: "UserBans",
                newName: "IsActive");

            migrationBuilder.RenameIndex(
                name: "IX_CamNguoiDung_IdNguoiThuHoi",
                table: "UserBans",
                newName: "IX_UserBans_RevokedByUserId");

            migrationBuilder.RenameIndex(
                name: "IX_CamNguoiDung_IdNguoiThucHien",
                table: "UserBans",
                newName: "IX_UserBans_ByUserId");

            migrationBuilder.RenameIndex(
                name: "IX_CamNguoiDung_IdNguoiDung",
                table: "UserBans",
                newName: "IX_UserBans_UserId");

            migrationBuilder.RenameColumn(
                name: "NgayTao",
                table: "ProjectBans",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "NgayCapNhat",
                table: "ProjectBans",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "MoTa",
                table: "ProjectBans",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "Lydo",
                table: "ProjectBans",
                newName: "Reason");

            migrationBuilder.RenameColumn(
                name: "IdNguoiThucHien",
                table: "ProjectBans",
                newName: "ByUserId");

            migrationBuilder.RenameColumn(
                name: "IdNguoiThuHoi",
                table: "ProjectBans",
                newName: "RevokedByUserId");

            migrationBuilder.RenameColumn(
                name: "IdDuAn",
                table: "ProjectBans",
                newName: "ProjectId");

            migrationBuilder.RenameColumn(
                name: "HieuLuc",
                table: "ProjectBans",
                newName: "IsActive");

            migrationBuilder.RenameIndex(
                name: "IX_CamDuAn_IdNguoiThuHoi",
                table: "ProjectBans",
                newName: "IX_ProjectBans_RevokedByUserId");

            migrationBuilder.RenameIndex(
                name: "IX_CamDuAn_IdNguoiThucHien",
                table: "ProjectBans",
                newName: "IX_ProjectBans_ByUserId");

            migrationBuilder.RenameIndex(
                name: "IX_CamDuAn_IdDuAn",
                table: "ProjectBans",
                newName: "IX_ProjectBans_ProjectId");

            migrationBuilder.RenameColumn(
                name: "NoiDung",
                table: "ProjectComments",
                newName: "Content");

            migrationBuilder.RenameColumn(
                name: "NgayTao",
                table: "ProjectComments",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "NgayCapNhat",
                table: "ProjectComments",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "IdTraLoi",
                table: "ProjectComments",
                newName: "ParentCommentId");

            migrationBuilder.RenameColumn(
                name: "IdNguoiTraLoi",
                table: "ProjectComments",
                newName: "RepliedUserId");

            migrationBuilder.RenameColumn(
                name: "IdNguoiDung",
                table: "ProjectComments",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "IdDuAn",
                table: "ProjectComments",
                newName: "ProjectId");

            migrationBuilder.RenameIndex(
                name: "IX_BinhLuanDuAn_IdTraLoi",
                table: "ProjectComments",
                newName: "IX_ProjectComments_ParentCommentId");

            migrationBuilder.RenameIndex(
                name: "IX_BinhLuanDuAn_IdNguoiTraLoi",
                table: "ProjectComments",
                newName: "IX_ProjectComments_RepliedUserId");

            migrationBuilder.RenameIndex(
                name: "IX_BinhLuanDuAn_IdNguoiDung",
                table: "ProjectComments",
                newName: "IX_ProjectComments_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_BinhLuanDuAn_IdDuAn",
                table: "ProjectComments",
                newName: "IX_ProjectComments_ProjectId");

            migrationBuilder.RenameColumn(
                name: "NgayTao",
                table: "ProjectReports",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "NgayCapNhat",
                table: "ProjectReports",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "MoTa",
                table: "ProjectReports",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "Lydo",
                table: "ProjectReports",
                newName: "Reason");

            migrationBuilder.RenameColumn(
                name: "IdNguoiThucHien",
                table: "ProjectReports",
                newName: "ByUserId");

            migrationBuilder.RenameColumn(
                name: "IdDuAn",
                table: "ProjectReports",
                newName: "ProjectId");

            migrationBuilder.RenameIndex(
                name: "IX_BaoCaoDuAn_IdNguoiThucHien",
                table: "ProjectReports",
                newName: "IX_ProjectReports_ByUserId");

            migrationBuilder.RenameIndex(
                name: "IX_BaoCaoDuAn_IdDuAn",
                table: "ProjectReports",
                newName: "IX_ProjectReports_ProjectId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetRoleClaims",
                table: "AspNetRoleClaims",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetRoles",
                table: "AspNetRoles",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetUserTokens",
                table: "AspNetUserTokens",
                columns: new[] { "UserId", "LoginProvider", "Name" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetUserLogins",
                table: "AspNetUserLogins",
                columns: new[] { "LoginProvider", "ProviderKey" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetUserClaims",
                table: "AspNetUserClaims",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetUserRoles",
                table: "AspNetUserRoles",
                columns: new[] { "UserId", "RoleId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetUsers",
                table: "AspNetUsers",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProjectLikes",
                table: "ProjectLikes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Projects",
                table: "Projects",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProjectCategories",
                table: "ProjectCategories",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserBans",
                table: "UserBans",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProjectBans",
                table: "ProjectBans",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProjectComments",
                table: "ProjectComments",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProjectReports",
                table: "ProjectReports",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId",
                principalTable: "AspNetRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                table: "AspNetUserClaims",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                table: "AspNetUserLogins",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId",
                principalTable: "AspNetRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                table: "AspNetUserRoles",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                table: "AspNetUserTokens",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectBans_AspNetUsers_ByUserId",
                table: "ProjectBans",
                column: "ByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectBans_AspNetUsers_RevokedByUserId",
                table: "ProjectBans",
                column: "RevokedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectBans_Projects_ProjectId",
                table: "ProjectBans",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectComments_AspNetUsers_RepliedUserId",
                table: "ProjectComments",
                column: "RepliedUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectComments_AspNetUsers_UserId",
                table: "ProjectComments",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectComments_ProjectComments_ParentCommentId",
                table: "ProjectComments",
                column: "ParentCommentId",
                principalTable: "ProjectComments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectComments_Projects_ProjectId",
                table: "ProjectComments",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectLikes_AspNetUsers_UserId",
                table: "ProjectLikes",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectLikes_Projects_ProjectId",
                table: "ProjectLikes",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectReports_AspNetUsers_ByUserId",
                table: "ProjectReports",
                column: "ByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectReports_Projects_ProjectId",
                table: "ProjectReports",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_AspNetUsers_UserId",
                table: "Projects",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_ProjectCategories_CategoryId",
                table: "Projects",
                column: "CategoryId",
                principalTable: "ProjectCategories",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserBans_AspNetUsers_ByUserId",
                table: "UserBans",
                column: "ByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserBans_AspNetUsers_RevokedByUserId",
                table: "UserBans",
                column: "RevokedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserBans_AspNetUsers_UserId",
                table: "UserBans",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
