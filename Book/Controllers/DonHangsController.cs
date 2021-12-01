﻿using System;
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
            var dt = db.Database.SqlQuery<ThongKe_Result>("ThongKe "+"'2021-11-23', '2021-11-24', 'CN1'");

            return View(dt.ToList());

        }

        // Thông kê số lượng theo ngày
        public ActionResult Chart2()
        {
            DateTime date = DateTime.Now;
            DateTime date2 = DateTime.Parse(date.ToString("MM-dd-yyyy"));
            var dt = db.Database.SqlQuery<ThongKe2_Result>("ThongKe2 '" +date2 +"' ,'2021-11-30'").ToList();

            int count = dt.Count;
            if(count <= 0)
            {
                ViewBag.Quantity = "empty";
            }
            return View(dt);

        }

        public ActionResult Chart3(string year)
        {
            year = year ?? "2021";
            int yy = Convert.ToInt32(year);
            ViewBag.year = year ?? "2021";
            var dt = db.Database.SqlQuery<trongnam_Result>("trongnam "+ yy);

            return View(dt.ToList());

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
