using WebS2TSTORE.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList.Mvc;


namespace WebS2TSTORE.Controllers
{
    public class DefaultController : Controller
    {
        S2TSTOREDataContext data = new S2TSTOREDataContext();
      
        //Index
        public ActionResult Index( )
        {
           
            return View();
        }
        //Category
        
        public ActionResult Category()
        {
            var listCategory = data.SanPhams.ToList();
            return View(listCategory);
        }
        public ActionResult Catalog()
        {
            var All_Cat = from th in data.ThuongHieus select th;
            return PartialView(All_Cat);
        }
        public ActionResult SPTheoThuongHieu(int id)
        {
            var giay = data.SanPhams.Where(n => n.maTH == id).ToList();
            return View(giay);
        }

        //Details Item
        public ActionResult Details(int id)
        {
            var detail = from d in data.SanPhams where d.maSP == id select d;
            return View(detail.Single());
        }
        private List<SanPham> sanPhamKhac(int id)
        {
            return data.SanPhams.OrderByDescending(b => b.maSP).Take(id).ToList();
        }
        public ActionResult More()
        {
            var SP_More = sanPhamKhac(4);
            return PartialView(SP_More);
        }
        public ActionResult LienHe()
        {
            return View();
        }
        [HttpPost]
        public ActionResult LienHe(FormCollection l, LienHe g)
        {

            var hoten = l["hoTen"];
            var email = l["email"];
            var dienthoai = l["sdt"];
            var tieude = l["tieuDe"];
            var noidung = l["noiDung"];

            if (String.IsNullOrEmpty(hoten))
            {
                ViewData["Loi1"] = "Họ tên không được bỏ trống";
            }

            if (String.IsNullOrEmpty(email))
            {
                ViewData["Loi2"] = "Phải nhập Email";
            }
            if (String.IsNullOrEmpty(noidung))
            {
                ViewData["Loi3"] = "Phải nhập nội dung";
            }
            else
            {


                g.hoTen = hoten;
                g.email = email;
                g.sdt = dienthoai;
                g.tieuDe = tieude;
                g.noiDung = noidung;
                data.LienHes.InsertOnSubmit(g);
                data.SubmitChanges();
                /*ViewBag.Thongbao = "Thank you for your feedback";*/
                return RedirectToAction("Index", "Default");
            }
            return this.LienHe();
        }
        /*Tin tức*/
        public ActionResult News()
        {
            var news = from tt in data.TinTucs select tt;
            return View(news);
        }
        /*Giới thiệu*/
        public ActionResult About()
        {
            return View();
        }
    }

}