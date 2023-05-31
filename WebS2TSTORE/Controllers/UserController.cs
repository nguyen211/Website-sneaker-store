using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebS2TSTORE.Models;

namespace WebS2TSTORE.Controllers
{
    public class UserController : Controller
    {
        S2TSTOREDataContext data = new S2TSTOREDataContext();
        [HttpGet]
        public ActionResult SignUp()
        {
            return View();
        }
        // POST: Hàm Dangky(post) Nhận dữ liệu từ trang Dangky và thực hiện việc tạo mới dữ liệu
        [HttpPost]
        public ActionResult SignUp(FormCollection f, KhachHang kh)
        {
            // Gán các giá tị người dùng nhập liệu cho các biến 
            var hoten = f["fullname"];
            var tendn = f["username"];
            var matkhau = f["password"];
            var matkhaunhaplai = f["passwordagain"];
            var dienthoai = f["phone"];
            var ngaysinh = String.Format("{0:MM/dd/yyyy}", f["dateofbirth"]);
            var diachi = f["addr"];
            var email = f["email"];
            if (String.IsNullOrEmpty(hoten))
            {
                /*ViewData["Loi1"] = "Họ tên khách hàng không được để trống";*/
            }
            else if (String.IsNullOrEmpty(tendn))
            {
                /*ViewData["Loi2"] = "Phải nhập tên đăng nhập";*/
            }
            else if (String.IsNullOrEmpty(matkhau))
            {
                /*ViewData["Loi3"] = "Phải nhập mật khẩu";*/
            }
            else if (String.IsNullOrEmpty(matkhaunhaplai))
            {
                /*ViewData["Loi4"] = "Phải nhập lại mật khẩu";*/
            }
            else if (String.IsNullOrEmpty(email))
            {
                /*ViewData["Loi5"] = "Email không được bỏ trống";*/
            }
            else if (String.IsNullOrEmpty(dienthoai))
            {
                /*ViewData["Loi6"] = "Phải nhập điện thoai";*/
            }else
            {
                //Gán giá trị cho đối tượng được tạo mới (kh)

                kh.hoTen = hoten;
                kh.taiKhoan = tendn;
                kh.matKhau = matkhau;
                kh.email = email;
                kh.diaChi = diachi;
                kh.sdt = dienthoai;
                kh.ngaySinh = DateTime.Parse(ngaysinh);
                data.KhachHangs.InsertOnSubmit(kh);
                data.SubmitChanges();
                return RedirectToAction("LogIn", "User");
            }
            return this.SignUp();
        }
        [HttpGet]
        public ActionResult LogIn()
        {
            return View();
        }
        [HttpPost]
        public ActionResult LogIn(FormCollection d)
        {
            // Gán các giá trị người dùng nhập liệu cho các biến 
            var tendn = d["login"];
            var matkhau = d["password"];
            if (String.IsNullOrEmpty(tendn))
            {
                /*ViewData["Loi1"] = "Phải nhập tên đăng nhập";*/
            }
            else if (String.IsNullOrEmpty(matkhau))
            {
                /*ViewData["Loi2"] = "Phải nhập mật khẩu";*/
            }
            else
            {
                //Gán giá trị cho đối tượng được tạo mới (kh)

                KhachHang kh = data.KhachHangs.SingleOrDefault(n => n.taiKhoan == tendn && n.matKhau == matkhau);
                if (kh != null)
                {
                    // ViewBag.Thongbao = "Chúc mừng đăng nhập thành công";
                    Session["Taikhoan"] = kh;
                    return RedirectToAction("Category", "Default");
                }
                else
                    ViewBag.Thongbao = "Tên đăng nhập hoặc mật khẩu không đúng";
            }
            return View();
        }
    }
}