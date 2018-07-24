using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SteppyNetAPI.WebAPI.Models.OpenMeasurement;
using SteppyNetAPI.WebAPI.Models;

namespace SteppyNetAPI.WebAPI.Controllers.OpenMeasurement
{
    public class ViewMeasurementController : Controller
    {
        private OpenMeasurementConnection db = new OpenMeasurementConnection();
        private STEPPY_APIContainer steppydb = new STEPPY_APIContainer();
        //
        // GET: /ViewMeasurement/

        public ActionResult Index(OpenDataMeasurementViewModel model)
        {
            var looseMeasurements = db.STEPPY_API_t_loose_measurement.ToList();
            var typeMeasurements = steppydb.STEPPY_API_m_measurement_type.ToList();
            var measurementData = from data in looseMeasurements
                                  join mtype in typeMeasurements
                                  on  data.measurement_type equals mtype.id_measurement_type
                                  select new OpenDataMeasurementViewModel()
                                  {
                                      IdMeasurement = data.id_measurement,
                                      UserId = data.id_user,
                                      IdMeasurementType = data.measurement_type,
                                      Type = mtype.measurement_type_name,
                                      MeasurementUnit = mtype.measurement_type_unit,
                                      TimeStamp = data.epoch_timestamp,
                                      InsertedTime = data.inserted_timestamp,
                                      Value = data.value,        
                                      DeviceType = data.device_type
                                  };

            if (model.UserId != null)
            {
                measurementData = measurementData.Where(x => x.UserId.ToUpper().Contains(model.UserId.ToUpper()));
            }

            if (model.Type != null)
            {
                measurementData = measurementData.Where(x => x.Type.ToUpper().Contains(model.Type.ToUpper()));
            }

            return View(measurementData.ToList());
        }

        //
        // GET: /ViewMeasurement/Details/5

        public ActionResult Details(long id = 0)
        {
            STEPPY_API_t_loose_measurement steppy_api_t_loose_measurement = db.STEPPY_API_t_loose_measurement.Find(id);
            if (steppy_api_t_loose_measurement == null)
            {
                return HttpNotFound();
            }
            return View(steppy_api_t_loose_measurement);
        }

        //
        // GET: /ViewMeasurement/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /ViewMeasurement/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(STEPPY_API_t_loose_measurement steppy_api_t_loose_measurement)
        {
            if (ModelState.IsValid)
            {
                db.STEPPY_API_t_loose_measurement.Add(steppy_api_t_loose_measurement);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(steppy_api_t_loose_measurement);
        }

        //
        // GET: /ViewMeasurement/Edit/5

        public ActionResult Edit(long id = 0)
        {
            STEPPY_API_t_loose_measurement steppy_api_t_loose_measurement = db.STEPPY_API_t_loose_measurement.Find(id);
            if (steppy_api_t_loose_measurement == null)
            {
                return HttpNotFound();
            }
            return View(steppy_api_t_loose_measurement);
        }

        //
        // POST: /ViewMeasurement/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(STEPPY_API_t_loose_measurement steppy_api_t_loose_measurement)
        {
            if (ModelState.IsValid)
            {
                db.Entry(steppy_api_t_loose_measurement).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(steppy_api_t_loose_measurement);
        }

        //
        // GET: /ViewMeasurement/Delete/5

        public ActionResult Delete(long id = 0)
        {
            STEPPY_API_t_loose_measurement steppy_api_t_loose_measurement = db.STEPPY_API_t_loose_measurement.Find(id);
            if (steppy_api_t_loose_measurement == null)
            {
                return HttpNotFound();
            }
            return View(steppy_api_t_loose_measurement);
        }

        //
        // POST: /ViewMeasurement/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            STEPPY_API_t_loose_measurement steppy_api_t_loose_measurement = db.STEPPY_API_t_loose_measurement.Find(id);
            db.STEPPY_API_t_loose_measurement.Remove(steppy_api_t_loose_measurement);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}