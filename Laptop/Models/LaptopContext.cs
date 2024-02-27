using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
//asknfkjsadn
namespace GiayDep.Models
{
    public partial class LaptopContext : IdentityDbContext<AppUserModel>
    {
        public LaptopContext()
        {
        }

        public LaptopContext(DbContextOptions<LaptopContext> options)
            : base(options)
        {
        }

        public virtual DbSet<CtHoaDon> CtHoaDons { get; set; } = null!;
        public virtual DbSet<CtPhieuNhap> CtPhieuNhaps { get; set; } = null!;
        public virtual DbSet<HoaDon> HoaDons { get; set; } = null!;
        public virtual DbSet<KhachHang> KhachHangs { get; set; } = null!;
        public virtual DbSet<LoaiSp> LoaiSps { get; set; } = null!;
        public virtual DbSet<Membership> Memberships { get; set; } = null!;
        public virtual DbSet<MembershipRight> MembershipRights { get; set; } = null!;
        public virtual DbSet<MembershipType> MembershipTypes { get; set; } = null!;
        public virtual DbSet<NhaCungCap> NhaCungCaps { get; set; } = null!;
        public virtual DbSet<NhaSanXuat> NhaSanXuats { get; set; } = null!;
        public virtual DbSet<NhanVien> NhanViens { get; set; } = null!;
        public virtual DbSet<PhieuNhap> PhieuNhaps { get; set; } = null!;
        public virtual DbSet<Right> Rights { get; set; } = null!;
        public virtual DbSet<SanPham> SanPhams { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.

                optionsBuilder.UseSqlServer("Data Source=DESKTOP-NF8GFOC\\SQLSERVER2022DEV;Initial Catalog=QLLAPTOP;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");

            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           

       

            modelBuilder.Entity<CtHoaDon>(entity =>
            {
                entity.HasKey(e => e.IdchitietDdh)
                    .HasName("PK_CT_HOA_DON_1");

                entity.ToTable("CT_HOA_DON");

                entity.Property(e => e.IdchitietDdh).HasColumnName("IDchitietDDH");

                entity.Property(e => e.Dongia).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.Idhoadon).HasColumnName("IDhoadon");

                entity.Property(e => e.Idsp).HasColumnName("IDsp");

                entity.Property(e => e.Tensp).HasMaxLength(50);

                entity.HasOne(d => d.IdhoadonNavigation)
                    .WithMany(p => p.CtHoaDons)
                    .HasForeignKey(d => d.Idhoadon)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CT_HOA_DON_HOA_DON");

                entity.HasOne(d => d.IdspNavigation)
                    .WithMany(p => p.CtHoaDons)
                    .HasForeignKey(d => d.Idsp)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CT_HOA_DON_SAN_PHAM");
            });

            modelBuilder.Entity<CtPhieuNhap>(entity =>
            {
                entity.HasKey(e => e.IdchitietPn)
                    .HasName("PK_CT_PHIEU_NHAP_1");

                entity.ToTable("CT_PHIEU_NHAP");

                entity.Property(e => e.IdchitietPn).HasColumnName("IDChitietPN");

                entity.Property(e => e.Gia).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.Idphieunhap).HasColumnName("IDphieunhap");

                entity.Property(e => e.Idsp).HasColumnName("IDSP");

                entity.HasOne(d => d.IdphieunhapNavigation)
                    .WithMany(p => p.CtPhieuNhaps)
                    .HasForeignKey(d => d.Idphieunhap)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CT_PHIEU_NHAP_PHIEU_NHAP");

                entity.HasOne(d => d.IdspNavigation)
                    .WithMany(p => p.CtPhieuNhaps)
                    .HasForeignKey(d => d.Idsp)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CT_PHIEU_NHAP_SAN_PHAM1");
            });

            modelBuilder.Entity<HoaDon>(entity =>
            {
                entity.HasKey(e => e.Idhoadon);

                entity.ToTable("HOA_DON");

                entity.Property(e => e.Idhoadon).HasColumnName("IDhoadon");

                entity.Property(e => e.Makh).HasMaxLength(450);

                entity.HasOne(d => d.MakhNavigation)
                    .WithMany(p => p.HoaDons)
                    .HasForeignKey(d => d.Makh)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_HOA_DON_AspNetUsers");
            });

            modelBuilder.Entity<KhachHang>(entity =>
            {
                entity.HasKey(e => e.Idkhachhang);

                entity.ToTable("KHACH_HANG");

                entity.Property(e => e.Idkhachhang).HasColumnName("IDkhachhang");

                entity.Property(e => e.Diachi).HasMaxLength(50);

                entity.Property(e => e.Email).HasMaxLength(50);

                entity.Property(e => e.Gioitinh).HasMaxLength(50);

                entity.Property(e => e.Idtv).HasColumnName("IDTV");

                entity.Property(e => e.Sđt).HasMaxLength(30);

                entity.Property(e => e.Tenkh).HasMaxLength(50);

                entity.HasOne(d => d.IdtvNavigation)
                    .WithMany(p => p.KhachHangs)
                    .HasForeignKey(d => d.Idtv)
                    .HasConstraintName("FK_KHACH_HANG_Membership");
            });

            modelBuilder.Entity<LoaiSp>(entity =>
            {
                entity.HasKey(e => e.Idloai);

                entity.ToTable("LoaiSp");

                entity.Property(e => e.Tenloai).HasMaxLength(50);
            });

            modelBuilder.Entity<Membership>(entity =>
            {
                entity.HasKey(e => e.Idtv);

                entity.ToTable("Membership");

                entity.Property(e => e.Idtv).HasColumnName("IDTV");

                entity.Property(e => e.DiaChi).HasMaxLength(150);

                entity.Property(e => e.Email).HasMaxLength(50);

                entity.Property(e => e.Gioitinh).HasMaxLength(50);

                entity.Property(e => e.HoTen).HasMaxLength(50);

                entity.Property(e => e.MaLoaiTv).HasColumnName("MaLoaiTV");

                entity.Property(e => e.Mk).HasMaxLength(100);

                entity.Property(e => e.Sđt)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Tk).HasMaxLength(100);

                entity.HasOne(d => d.MaLoaiTvNavigation)
                    .WithMany(p => p.Memberships)
                    .HasForeignKey(d => d.MaLoaiTv)
                    .HasConstraintName("FK_Membership_Membership_Type");
            });

            modelBuilder.Entity<MembershipRight>(entity =>
            {
                entity.HasKey(e => new { e.MaLoaiTv, e.Idright })
                    .HasName("PK_Membership_right_1");

                entity.ToTable("Membership_right");

                entity.Property(e => e.MaLoaiTv).HasColumnName("MaLoaiTV");

                entity.Property(e => e.Idright)
                    .HasMaxLength(50)
                    .HasColumnName("IDright");

                entity.Property(e => e.Note).HasMaxLength(255);

                entity.HasOne(d => d.IdrightNavigation)
                    .WithMany(p => p.MembershipRights)
                    .HasForeignKey(d => d.Idright)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Membership_right_Right");

                entity.HasOne(d => d.MaLoaiTvNavigation)
                    .WithMany(p => p.MembershipRights)
                    .HasForeignKey(d => d.MaLoaiTv)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Membership_right_Membership_Type");
            });

            modelBuilder.Entity<MembershipType>(entity =>
            {
                entity.HasKey(e => e.MaLoaiTv);

                entity.ToTable("Membership_Type");

                entity.Property(e => e.MaLoaiTv)
                    .ValueGeneratedNever()
                    .HasColumnName("MaLoaiTV");

                entity.Property(e => e.TenLoai).HasMaxLength(50);
            });

            modelBuilder.Entity<NhaCungCap>(entity =>
            {
                entity.HasKey(e => e.Idnhacc);

                entity.ToTable("NHA_CUNG_CAP");

                entity.Property(e => e.Idnhacc).HasColumnName("IDnhacc");

                entity.Property(e => e.Diachi).HasMaxLength(50);

                entity.Property(e => e.Email).HasMaxLength(50);

                entity.Property(e => e.Idnhasx).HasColumnName("IDnhasx");

                entity.Property(e => e.Sdt).HasMaxLength(30);

                entity.Property(e => e.Tennhacc).HasMaxLength(50);

                entity.HasOne(d => d.IdnhasxNavigation)
                    .WithMany(p => p.NhaCungCaps)
                    .HasForeignKey(d => d.Idnhasx)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_NHA_CUNG_CAP_NHA_SAN_XUAT");
            });

            modelBuilder.Entity<NhaSanXuat>(entity =>
            {
                entity.HasKey(e => e.Idnhasx);

                entity.ToTable("NHA_SAN_XUAT");

                entity.Property(e => e.Idnhasx).HasColumnName("IDnhasx");

                entity.Property(e => e.Diachi).HasMaxLength(50);

                entity.Property(e => e.Email).HasMaxLength(50);

                entity.Property(e => e.Sđt).HasMaxLength(30);

                entity.Property(e => e.Tennhasx).HasMaxLength(50);
            });

            modelBuilder.Entity<NhanVien>(entity =>
            {
                entity.HasKey(e => e.Idnhanvien);

                entity.ToTable("NHAN_VIEN");

                entity.Property(e => e.Idnhanvien)
                    .HasMaxLength(50)
                    .HasColumnName("IDnhanvien");

                entity.Property(e => e.Diachi).HasMaxLength(50);

                entity.Property(e => e.Email).HasMaxLength(50);

                entity.Property(e => e.Gioitinh).HasMaxLength(50);

                entity.Property(e => e.Ngaysinh).HasColumnType("datetime");

                entity.Property(e => e.Sdt).HasMaxLength(30);

                entity.Property(e => e.Tennv).HasMaxLength(50);
            });

            modelBuilder.Entity<PhieuNhap>(entity =>
            {
                entity.HasKey(e => e.Idphieunhap);

                entity.ToTable("PHIEU_NHAP");

                entity.Property(e => e.Idphieunhap).HasColumnName("IDphieunhap");

                entity.Property(e => e.Idnhacc).HasColumnName("IDnhacc");

                entity.HasOne(d => d.IdnhaccNavigation)
                    .WithMany(p => p.PhieuNhaps)
                    .HasForeignKey(d => d.Idnhacc)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PHIEU_NHAP_NHA_CUNG_CAP");
            });

            modelBuilder.Entity<Right>(entity =>
            {
                entity.HasKey(e => e.Idright);

                entity.ToTable("Right");

                entity.Property(e => e.Idright)
                    .HasMaxLength(50)
                    .HasColumnName("IDright");

                entity.Property(e => e.NameRight).HasMaxLength(50);
            });

            modelBuilder.Entity<SanPham>(entity =>
            {
                entity.HasKey(e => e.Idsp);

                entity.ToTable("SAN_PHAM");

                entity.Property(e => e.Idsp).HasColumnName("IDSP");

                entity.Property(e => e.Baohanh).HasMaxLength(50);

                entity.Property(e => e.Description).HasMaxLength(100);

                entity.Property(e => e.Khuyenmai).HasMaxLength(50);

                entity.Property(e => e.Tensp).HasMaxLength(50);

                entity.HasOne(d => d.MaloaispNavigation)
                    .WithMany(p => p.SanPhams)
                    .HasForeignKey(d => d.Maloaisp)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SAN_PHAM_LoaiSp");

                entity.HasOne(d => d.ManhaccNavigation)
                    .WithMany(p => p.SanPhams)
                    .HasForeignKey(d => d.Manhacc)
                    .HasConstraintName("FK_SAN_PHAM_NHA_CUNG_CAP");
            });

            base.OnModelCreating(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
