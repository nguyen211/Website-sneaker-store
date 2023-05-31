using WebS2TSTORE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebS2TSTORE.Models
{
    public class Cart
    {
        S2TSTOREDataContext data = new S2TSTOREDataContext();
        public int gMaSP { set; get; }
        public string gTenSP { set; get; }
        public string gImage { set; get; }
        public int gGiaBan { set; get; }
        public int gSoLuong { set; get; }
        public int gSize { set; get; }
        public double gTong { get { return gGiaBan * gSoLuong; } }
        public Cart(int MaSP)
        {
            gMaSP = MaSP;
            SanPham sp = data.SanPhams.Single(n => n.maSP == gMaSP);
            gTenSP = sp.tenSP;
            gImage = sp.image;
            gSize = int.Parse(sp.size.ToString());
            gGiaBan = int.Parse(sp.giaBan.ToString());
            gSoLuong = 1;
        }
    }
}