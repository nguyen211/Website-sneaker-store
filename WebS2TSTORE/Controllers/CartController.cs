using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebS2TSTORE.Models;

namespace WebS2TSTORE.Controllers
{
    public class CartController : Controller
    {
        S2TSTOREDataContext data = new S2TSTOREDataContext();
        //Lay gio hang
        public List<Cart> Laygiohang()
        {
            List<Cart> listGiohang = Session["Cart"] as List<Cart>;
            if (listGiohang == null)
            {
                //Neu gio hang chua ton tai thi khoi tao listGiohang
                listGiohang = new List<Cart>();
                Session["Cart"] = listGiohang;
            }
            return listGiohang;
        }
        //Them hang vao gio
        public ActionResult Themgiohang(int gMaSP, string strURL)
        {
            //Lay ra Session gio hang
            List<Cart> listGiohang = Laygiohang();
            //Kiem tra sách này tồn tại trong Session["Giohang"] chưa?
            Cart sanpham = listGiohang.Find(n => n.gMaSP == gMaSP);
            if (sanpham == null)
            {
                sanpham = new Cart(gMaSP);
                listGiohang.Add(sanpham);
                return Redirect(strURL);
            }
            else
            {
                sanpham.gSoLuong++;
                return Redirect(strURL);
            }
        }
        //Tong so luong
        private int TongSoLuong()
        {
            int gTongSoLuong = 0;
            List<Cart> listGiohang = Session["Cart"] as List<Cart>;
            if (listGiohang != null)
            {
                gTongSoLuong = listGiohang.Sum(n => n.gSoLuong);
            }
            return gTongSoLuong;
        }
        //Tinh tong tien
        private double TongTien()
        {
            double iTongTien = 0;
            List<Cart> listGiohang = Session["Cart"] as List<Cart>;
            if (listGiohang != null)
            {
                iTongTien = listGiohang.Sum(n => n.gTong);
            }
            return iTongTien;
        }
        //Xay dung trang Gio hang
        public ActionResult Cart()
        {
            List<Cart> listGiohang = Laygiohang();
            if (listGiohang.Count == 0)
            {
                return RedirectToAction("Category", "Default");
            }
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();
            return View(listGiohang);
        }
        public ActionResult CartPartial()
        {
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();
            return PartialView();
        }
        //Xoa Giohang
        public ActionResult XoaGiohang(int gMaProduct)
        {
            //Lay gio hang tu Session
            List<Cart> listGiohang = Laygiohang();
            //Kiem tra sach da co trong Session["Cart"]
            Cart sanpham = listGiohang.SingleOrDefault(n => n.gMaSP == gMaProduct);
            //Neu ton tai thi cho sua Soluong
            if (sanpham != null)
            {
                listGiohang.RemoveAll(n => n.gMaSP == gMaProduct);
                return RedirectToAction("Cart");

            }
            if (listGiohang.Count == 0)
            {
                return RedirectToAction("Category", "Default");
            }
            return RedirectToAction("Cart");
        }
        //Cap nhat Giỏ hàng
        public ActionResult CapnhatGiohang(int gIDProduct, FormCollection f)
        {

            //Lay gio hang tu Session
            List<Cart> listGiohang = Laygiohang();
            //Kiem tra sach da co trong Session["Cart"]
            Cart sanpham = listGiohang.SingleOrDefault(n => n.gMaSP == gIDProduct);
            //Neu ton tai thi cho sua Soluong
            if (sanpham != null)
            {
                sanpham.gSoLuong = int.Parse(f["txtSoluong"].ToString());
            }
            return RedirectToAction("Cart");
        }
        //Xoa tat ca thong tin trong Gio hang
        public ActionResult XoaTatcaGiohang()
        {
            //Lay gio hang tu Session
            List<Cart> listGiohang = Laygiohang();
            listGiohang.Clear();
            return RedirectToAction("Category", "Default");
        }
        //Hien thi View DatHang de cap nhat cac thong tin cho Don hang
        [HttpGet]
        public ActionResult Order()
        {
            //Kiem tra dang nhap
            if (Session["Taikhoan"] == null || Session["Taikhoan"].ToString() == "")
            {
                return RedirectToAction("LogIn", "User");
            }
            if (Session["Cart"] == null)
            {
                return RedirectToAction("Category", "Default");
            }

            //Lay gio hang tu Session
            List<Cart> listGiohang = Laygiohang();
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();

            return View(listGiohang);
        }
        //Xay dung chuc nang Dathang
        [HttpPost]
        public ActionResult Order(FormCollection collection)
        {           
            //Them Don hang
            DatHang dh = new DatHang();
            KhachHang kh =(KhachHang) Session["Taikhoan"];
            List<Cart> gh = Laygiohang();
            dh.idKH = kh.idKH;            
            dh.ngayDat = DateTime.Now;
            var ngaygiao = String.Format("{0:MM/dd/yyyy}", collection["Ngaygiao"]);
            dh.ngayGiao =DateTime.Parse(ngaygiao);
            dh.tinhTrangGiaoHang = dh.tinhTrangGiaoHang;
            dh.daThanhToan = false;
            data.DatHangs.InsertOnSubmit(dh);
            data.SubmitChanges();
            //Them chi tiet don hang            
            foreach(var item in gh)            
            {
                ChiTietDonHang ctdh = new ChiTietDonHang();
                ctdh.maGD = dh.maGD;
                ctdh.maSP = item.gMaSP;
                ctdh.soLuong =item.gSoLuong;
                ctdh.tongDH =(int) item.gTong;
                data.ChiTietDonHangs.InsertOnSubmit(ctdh);
            }            
            data.SubmitChanges();
            Session["Cart"] = null;
            return RedirectToAction("Confirm", "Cart");
        }
        public ActionResult Confirm()
        {
            return View();
        }
    }
}