using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Book.Models;

namespace Book.Controllers
{
    public class DonHangsController : Controller
    {
        private DoAnEntities1 db = new DoAnEntities1();

        // GET: DonHangs
        public ActionResult Index()
        {

            var donHangs = db.DonHangs.Include(d => d.KhachHang);
            return View(donHangs.ToList());
        }

        public ActionResult Chart()
        {
            DateTime date = DateTime.Now;

            var dt = db.Database.SqlQuery<trongtuan_Result>("trongtuan '" + date + "'");
            return View(dt.ToList());

        }

        // Thông kê số lượng theo ngày
        public ActionResult Chart2()
        {
            DateTime date = DateTime.Now;

            var dt = db.Database.SqlQuery<ThongKe2_Result>("ThongKe2 '" + date + "'").ToList();
            int count1 = dt.Count;
            if (count1 <= 0)
            {
                var date2 = DateTime.Today.ToString("dd-MM-yyyy");
                var dt2 = db.Database.SqlQuery<ThongKe2_Result>("ThongKe2 '" + date2 + "'").ToList();
                int count = dt2.Count;
                if (count <= 0)
                {
                    ViewBag.Quantity = null;
                }
                else
                {
                    ViewBag.Quantity = "have";

                }
                return View(dt2);
            }
            else
            {
                ViewBag.Quantity = "have";
            }

            return View(dt);
        }

        public ActionResult Chart3()
        {
            string year = null;

            if (TempData["yyyy"] != null)
            {
                year = TempData["yyyy"].ToString();
            }

            DateTime current = DateTime.Now;

            year = year ?? current.Year.ToString();

            int yy = Convert.ToInt32(year);

            var dt2 = db.Database.SqlQuery<bookyear2_Result>("bookyear2 ").ToList();

            List<int> allYears = new List<int>();
            for (int i = 0; i < dt2.Count(); i++)
            {
                allYears.Add((int)dt2[i].nam);
            }
            ViewBag.time = allYears;

            ViewBag.year = yy;


            var dt = db.Database.SqlQuery<trongnam_Result>("trongnam " + yy);



            return View(dt.ToList());

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Chart3(string year, string type)
        {
            TempData["yyyy"] = year;
            Session["YYYY"] = year;
            if (type == "soluong")
            {
                return RedirectToAction("SoLuongTheoNam");
            }
            else
            {
                return RedirectToAction("Chart3");
            }

        }


        public ActionResult Chart4()
        {

            var dt2 = db.Database.SqlQuery<bookyear2_Result>("bookyear2 ").ToList();

            List<int> allYears = new List<int>();
            for (int i = 0; i < dt2.Count(); i++)
            {
                allYears.Add((int)dt2[i].nam);
            }
            ViewBag.time = allYears;

            if (TempData["d"] == null)
            {
                DateTime date = DateTime.Now;
                var dt = db.Database.SqlQuery<trongthang_Result>("trongthang '" + date + "'").ToList();

                return View(dt);
            }
            else
            {
                string d = TempData["d"].ToString();
                var dt = db.Database.SqlQuery<trongthang_Result>("trongthang '" + d + "'").ToList();
                return View(dt);
            }

        }
        public ActionResult SoLuongTheoTuan()
        {
            DateTime date1 = DateTime.Now;

            var dt = db.Database.SqlQuery<SoSachBanTrongTuan_Result>("SoSachBanTrongTuan '" + date1 + "'").ToList();
            return View(dt);

        }

        public ActionResult SoLuongTheoNam()
        {
            string yy = TempData["yyyy"].ToString();
            var dt = db.Database.SqlQuery<SoSachBanTrongNam_Result>("SoSachBanTrongNam '" + yy + "-1-1'").ToList();
            return View(dt);

        }

        public ActionResult SoLuongTheoThang()
        {
            DateTime date2 = new DateTime();
            string date = date2.ToString();
            if (TempData["d"] != null)
            {

                date = TempData["d"].ToString();
            }
            var dt = db.Database.SqlQuery<SoSachBanTrongThang_Result>("SoSachBanTrongThang '" + date + "'").ToList();
            return View(dt);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ThongKeTheoThang(string month, string year, string type)
        {
            string date = year + "-" + month + "-1";
            TempData["d"] = date;
            Session["month"] = month;
            if (type == "doanhthu")
            {
                return RedirectToAction("Chart4");
            }
            else
            {
                return RedirectToAction("SoLuongTheoThang");
            }

        }


        // Update cập nhật mua hàng
        public ActionResult Update(DonHang dh)
        {
            if (dh.NgayNhan != null)
            {
                Session["appear"] = "have";
            }
            db.Entry(dh).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index", "Saches");
        }

        // GET: DonHangs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DonHang donHang = db.DonHangs.Find(id);
            if (donHang == null)
            {
                return HttpNotFound();
            }
            return View(donHang);
        }

        // GET: DonHangs/Create
        public ActionResult Create()
        {
            ViewBag.MaKH = new SelectList(db.KhachHangs, "MaKH", "HovaTen");
            return View();
        }

        // POST: DonHangs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaDH,MaKH,NgayDat,NgayNhan,TongTien")] DonHang donHang)
        {
            if (ModelState.IsValid)
            {

                db.DonHangs.Add(donHang);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MaKH = new SelectList(db.KhachHangs, "MaKH", "HovaTen", donHang.MaKH);
            return View(donHang);
        }

        // GET: DonHangs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DonHang donHang = db.DonHangs.Find(id);
            if (donHang == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaKH = new SelectList(db.KhachHangs, "MaKH", "HovaTen", donHang.MaKH);
            return View(donHang);
        }

        // POST: DonHangs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaDH,MaKH,NgayDat,NgayNhan,TongTien")] DonHang donHang)
        {
            if (ModelState.IsValid)
            {
                db.Entry(donHang).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MaKH = new SelectList(db.KhachHangs, "MaKH", "HovaTen", donHang.MaKH);
            return View(donHang);
        }

        // GET: DonHangs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DonHang donHang = db.DonHangs.Find(id);
            if (donHang == null)
            {
                return HttpNotFound();
            }
            return View(donHang);
        }

        // POST: DonHangs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DonHang donHang = db.DonHangs.Find(id);
            db.DonHangs.Remove(donHang);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }


}