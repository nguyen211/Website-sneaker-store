using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;

using WebS2TSTORE.Models;
using PagedList;
using PagedList.Mvc;

namespace WebS2TSTORE.Controllers
{

    public class AdminController : Controller
    {
        S2TSTOREDataContext data = new S2TSTOREDataContext();
        // GET: Admin
        /// <summary>
        /// Đăng nhập vào TRANG QUẢN TRỊ
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult LoginAdminator()
        {
            return View();
        }
        [HttpPost]
        public ActionResult LoginAdminator(FormCollection a)
        {
            // Gán các giá trị người dùng nhập liệu cho các biến 
            var taiKhoan = a["userName"];
            var matKhau = a["passWord"];
            if (String.IsNullOrEmpty(taiKhoan))
            {
                /*ViewData["Loi1"] = "Phải nhập tên đăng nhập";*/
            }
            else if (String.IsNullOrEmpty(matKhau))
            {
                /*ViewData["Loi2"] = "Phải nhập mật khẩu";*/
            }
            else
            {
                //Gán giá trị cho đối tượng được tạo mới (ad)        

                Account ad = data.Accounts.SingleOrDefault(n => n.userName == taiKhoan && n.password == matKhau);
                if (ad != null)
                {
                    // ViewBag.Thongbao = "Chúc mừng đăng nhập thành công";
                    Session["Taikhoanadmin"] = ad;
                    return RedirectToAction("Index", "Admin");
                }
                else
                    ViewBag.Thongbao = "Tên đăng nhập hoặc mật khẩu không đúng";
            }
            return View();
        }
        /// <summary>
        /// Quản lý SẢN PHẨM
        /// </summary>
        /// <returns></returns>
        /*Index Sản phẩm*/
        public ActionResult index()
        {
            return View(data.SanPhams.ToList());
        }
        /*Tạo SẢN PHẨM*/
        [HttpGet]
        public ActionResult CreateProduct()
        {
            //Dua du lieu vao dropdownList
            //Lay ds tu tabke chu de, sắp xep tang dan trheo ten chu de, chon lay gia tri Ma CD, hien thi thi Tenchude
            ViewBag.MaTH = new SelectList(data.ThuongHieus.ToList().OrderBy(n => n.tenTH), "maTH", "tenTH");

            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult CreateProduct(SanPham giay, HttpPostedFileBase fileUpload, HttpPostedFileBase fileUpload1, HttpPostedFileBase fileUpload2, HttpPostedFileBase fileUpload3, HttpPostedFileBase fileUpload4)
        {
            ViewBag.MaTH = new SelectList(data.ThuongHieus.ToList().OrderBy(n => n.tenTH), "maTH", "tenTH");
            if (fileUpload == null)
            {
                ViewBag.Thongbao = "Vui lòng chọn ảnh chính";
                return View();
            }
            else
            {
                if (ModelState.IsValid)
                {
                    //Luu ten fie, luu y bo sung thu vien using System.IO;
                    var fileName = Path.GetFileName(fileUpload.FileName);
                    //Luu duong dan cua file
                    var path = Path.Combine(Server.MapPath("~/Content/images"), fileName);

                    var fileName1 = Path.GetFileName(fileUpload1.FileName);
                    var path1 = Path.Combine(Server.MapPath("~/Content/images"), fileName1);
                    var fileName2 = Path.GetFileName(fileUpload2.FileName);
                    var path2 = Path.Combine(Server.MapPath("~/Content/images"), fileName2);
                    var fileName3 = Path.GetFileName(fileUpload3.FileName);
                    var path3 = Path.Combine(Server.MapPath("~/Content/images"), fileName3);
                    var fileName4 = Path.GetFileName(fileUpload4.FileName);
                    var path4 = Path.Combine(Server.MapPath("~/Content/images"), fileName4);
                    if (System.IO.File.Exists(path))
                        ViewBag.Thongbao = "Hình ảnh đã tồn tại";
                    else
                    {
                        //Luu hinh anh vao duong dan
                        fileUpload.SaveAs(path);
                        fileUpload1.SaveAs(path1);
                        fileUpload2.SaveAs(path2);
                        fileUpload3.SaveAs(path3);
                        fileUpload4.SaveAs(path4);

                    }
                    giay.image = fileName;
                    giay.image1 = fileName1;
                    giay.image2 = fileName2;
                    giay.image3 = fileName3;
                    giay.image4 = fileName4;
                    //Luu vao CSDL
                    data.SanPhams.InsertOnSubmit(giay);
                    data.SubmitChanges();
                }
                return RedirectToAction("Index");
            }
        }
        /*Chi tiết SẢN PHẨM*/
        public ActionResult DetailPro(int id)
        {
            //Lay ra doi tuong sach theo ma
            SanPham giay = data.SanPhams.SingleOrDefault(n => n.maSP == id);
            ViewBag.Masach = giay.maSP;
            if (giay == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(giay);
        }
        /*Xóa SẢN PHẨM*/
        [HttpGet]
        public ActionResult DelPro(int id)
        {
            //Lay ra doi tuong sach can xoa theo ma
            SanPham giay = data.SanPhams.SingleOrDefault(n => n.maSP == id);
            ViewBag.maSP = giay.maSP;
            if (giay == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(giay);
        }

        [HttpPost, ActionName("DelPro")]
        public ActionResult ConfirmDel(int id)
        {
            //Lay ra doi tuong sach can xoa theo ma
            SanPham giay = data.SanPhams.SingleOrDefault(n => n.maSP == id);
            ViewBag.Masach = giay.maSP;
            if (giay == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            data.SanPhams.DeleteOnSubmit(giay);
            data.SubmitChanges();
            return RedirectToAction("Index");
        }
        /*Sửa SẢN PHẨM*/
        [HttpGet]
        public ActionResult EditPro(int id)
        {
            //Lay ra doi tuong sach theo ma
            SanPham giay = data.SanPhams.SingleOrDefault(n => n.maSP == id);
            ViewBag.maSP = giay.maSP;
            if (giay == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            //Dua du lieu vao dropdownList
            //Lay ds tu tabke chu de, sắp xep tang dan trheo ten chu de, chon lay gia tri Ma CD, hien thi thi Tenchude
            ViewBag.maTH = new SelectList(data.ThuongHieus.ToList().OrderBy(n => n.tenTH), "maTH", "tenTH", giay.maTH);
            return View(giay);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult EditPro(SanPham giay, HttpPostedFileBase fileUpload, HttpPostedFileBase fileUpload1, HttpPostedFileBase fileUpload2, HttpPostedFileBase fileUpload3, HttpPostedFileBase fileUpload4)
        {
            ViewBag.MaTH = new SelectList(data.ThuongHieus.ToList().OrderBy(n => n.tenTH), "maTH", "tenTH");
            if (fileUpload == null)
            {
                ViewBag.Thongbao = "Vui lòng chọn ảnh chính";
                return View();
            }
            else
            {
                if (ModelState.IsValid)
                {
                    //Luu ten fie, luu y bo sung thu vien using System.IO;
                    var fileName = Path.GetFileName(fileUpload.FileName);
                    //Luu duong dan cua file
                    var path = Path.Combine(Server.MapPath("~/Content/images"), fileName);

                    var fileName1 = Path.GetFileName(fileUpload1.FileName);
                    var path1 = Path.Combine(Server.MapPath("~/Content/images"), fileName1);
                    var fileName2 = Path.GetFileName(fileUpload2.FileName);
                    var path2 = Path.Combine(Server.MapPath("~/Content/images"), fileName2);
                    var fileName3 = Path.GetFileName(fileUpload3.FileName);
                    var path3 = Path.Combine(Server.MapPath("~/Content/images"), fileName3);
                    var fileName4 = Path.GetFileName(fileUpload4.FileName);
                    var path4 = Path.Combine(Server.MapPath("~/Content/images"), fileName4);
                    if (System.IO.File.Exists(path))
                        ViewBag.Thongbao = "Hình ảnh đã tồn tại";
                    else
                    {
                        //Luu hinh anh vao duong dan
                        fileUpload.SaveAs(path);
                        fileUpload1.SaveAs(path1);
                        fileUpload2.SaveAs(path2);
                        fileUpload3.SaveAs(path3);
                        fileUpload4.SaveAs(path4);

                    }
                    giay.image = fileName;
                    giay.image1 = fileName1;
                    giay.image2 = fileName2;
                    giay.image3 = fileName3;
                    giay.image4 = fileName4;
                    //Luu vao CSDL   
                    UpdateModel(giay);
                    data.SubmitChanges();

                }
                return RedirectToAction("Index");
            }
        }
        /// <summary>
        /// Quản lý Tin tức
        /// </summary>
        /// <returns></returns>
        /// 
        public ActionResult News()
        {
            return View(data.TinTucs.ToList());
        }
        /*Tạo TIN*/
        [HttpGet]
        public ActionResult CreateNews()
        {
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult CreateNews(TinTuc tin, HttpPostedFileBase fileUpload)
        {
            //Kiem tra duong dan file
            if (fileUpload == null)
            {
                ViewBag.Thongbao = "Vui lòng chọn ảnh bìa";
                return View();
            }
            //Them vao CSDL
            else
            {
                if (ModelState.IsValid)
                {
                    //Luu ten fie, luu y bo sung thu vien using System.IO;
                    var fileName = Path.GetFileName(fileUpload.FileName);
                    //Luu duong dan cua file
                    var path = Path.Combine(Server.MapPath("~/Content/images"), fileName);
                    //Kiem tra hình anh ton tai chua?
                    if (System.IO.File.Exists(path))
                        ViewBag.Thongbao = "Hình ảnh đã tồn tại";
                    else
                    {
                        //Luu hinh anh vao duong dan
                        fileUpload.SaveAs(path);
                    }
                    tin.image = fileName;
                    //Luu vao CSDL
                    data.TinTucs.InsertOnSubmit(tin);
                    data.SubmitChanges();
                }
                return RedirectToAction("News");
            }
        }
        /*Chi tiết TIN*/
        public ActionResult DetailNews(int id)
        {
            //Lay ra doi tuong sach theo ma
            TinTuc news = data.TinTucs.SingleOrDefault(n => n.maTT == id);
            ViewBag.maTT = news.maTT;
            if (news == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(news);
        }
        /*Xóa TIN*/
        [HttpGet]
        public ActionResult DelNews(int id)
        {
            //Lay ra doi tuong sach can xoa theo ma
            TinTuc news = data.TinTucs.SingleOrDefault(n => n.maTT == id);
            ViewBag.maTT = news.maTT;
            if (news == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(news);
        }

        [HttpPost, ActionName("DelNews")]
        public ActionResult ConfirmDelNews(int id)
        {
            //Lay ra doi tuong sach can xoa theo ma
            TinTuc news = data.TinTucs.SingleOrDefault(n => n.maTT == id);
            ViewBag.MaTT = news.maTT;
            if (news == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            data.TinTucs.DeleteOnSubmit(news);
            data.SubmitChanges();
            return RedirectToAction("News");
        }
        /// <summary>
        /// Quản lý Liên hệ
        /// </summary>
        /// <returns></returns>
        /// 
        public ActionResult Contact()
        {
            return View(data.LienHes.ToList());
        }
        /*Chi tiết Liên hệ*/
        public ActionResult DetailCon(int id)
        {
            //Lay ra doi tuong sach theo ma
            LienHe lienHe = data.LienHes.SingleOrDefault(n => n.maLH == id);
            ViewBag.maLH = lienHe.maLH;
            if (lienHe == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(lienHe);
        }
        /*Xóa Liên hệ*/
        [HttpGet]
        public ActionResult DelCon(int id)
        {
            //Lay ra doi tuong sach can xoa theo ma
            LienHe lienHe = data.LienHes.SingleOrDefault(n => n.maLH == id);
            ViewBag.maLH = lienHe.maLH;
            if (lienHe == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(lienHe);
        }

        [HttpPost, ActionName("DelCon")]
        public ActionResult ConfirmDelCon(int id)
        {
            //Lay ra doi tuong sach can xoa theo ma
            LienHe lienHe = data.LienHes.SingleOrDefault(n => n.maLH == id);
            ViewBag.maLH = lienHe.maLH;
            if (lienHe == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            data.LienHes.DeleteOnSubmit(lienHe);
            data.SubmitChanges();
            return RedirectToAction("Contact");
        }
        
    }
}