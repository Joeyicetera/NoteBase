﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NoteBaseLogicFactory;
using NoteBaseLogicInterface.Models;
using NoteBaseLogicInterface;
using System.Configuration;
using App.Models;
using Microsoft.AspNetCore.Authorization;
using System.Data.Common;
using System;
using System.Dynamic;

namespace App.Controllers
{
    [Authorize]
    public class CategoryController : Controller
    {
        private readonly IConfiguration _config;
        private readonly string connString;
        private readonly IPersonProcessor personProcessor;
        private readonly ICategoryProcessor categoryProcessor;
        private Person? person;

        public CategoryController(IConfiguration configuration)
        {
            _config = configuration;
            connString = _config.GetConnectionString("NoteBaseConnString");
            personProcessor = ProcessorFactory.CreatePersonProcessor(connString);
            categoryProcessor = ProcessorFactory.CreateCategoryProcessor(connString);
        }

        // GET: Category/Details/5
        public ActionResult Details(int id)
        {
            Category category = categoryProcessor.GetById(id);

            if (category.ID == 0)
            {
                ViewBag.Succeeded = false;
                ViewBag.Message = "Deze categorie bestaat niet";

                return View();
            }

            INoteProcessor noteProcessor = ProcessorFactory.CreateNoteProcessor(connString);
            category.noteList = noteProcessor.GetByCategory(id);

            ViewBag.Succeeded = true;

            return View(new CategoryModel(category));

        }

        // GET: Category/Create
        public ActionResult Create()
        {
            ViewBag.Succeeded = true;
            return View();
        }

        // POST: Category/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            if (!categoryProcessor.IsValidTitle(collection["Title"]))
            {
                ViewBag.Succeeded = false;
                ViewBag.Message = "Titel mag niet leeg zijn";


                return View();
            }
            if (!categoryProcessor.IsTitleUnique(collection["Title"]))
            {
                ViewBag.Succeeded = false;
                ViewBag.Message = "Er bestaat al een categorie met deze titel";

                return View();
            }

            this.person = personProcessor.GetByEmail(User.Identity.Name);

            Category category = categoryProcessor.Create(collection["Title"], person.ID);

            if (category.ID == 0)
            {
                ViewBag.Succeeded = false;

                return View();
            }

            ViewBag.Succeeded = true;

            //for somereason when redirecting the details try to get the item before it has been added to database
            return RedirectToAction(nameof(Details), category.ID);
        }

        // GET: Category/Edit/5
        public ActionResult Edit(int id)
        {
            Category category = categoryProcessor.GetById(id);

            if (category.ID == 0)
            {
                ViewBag.Succeeded = false;
                ViewBag.Message = "Deze categorie bestaat niet";

                return View();
            }

            ViewBag.Succeeded = true;

            return View(new CategoryModel(category));
        }

        // POST: Category/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            if (!categoryProcessor.IsValidTitle(collection["Title"]))
            {
                ViewBag.Succeeded = false;
                ViewBag.Message = "Titel mag niet leeg zijn";

                return View();
            }
            if (!categoryProcessor.IsTitleUnique(collection["Title"]))
            {
                ViewBag.Succeeded = false;
                ViewBag.Message = "Er bestaat al een categorie met deze titel";

                return View();
            }
            if (!categoryProcessor.DoesCategoryExits(id))
            {
                ViewBag.Succeeded = false;
                ViewBag.Message = "De categorie met dit id bestaat niet";

                return View();
            }

            this.person = personProcessor.GetByEmail(User.Identity.Name);

            Category category = categoryProcessor.Update(id, collection["Title"], person.ID);

            if (category.ID == 0)
            {
                ViewBag.Succeeded = false;
                ViewBag.Message = "";

                return View();
            }

            ViewBag.Succeeded = true;

            //diffrent redirect options?
            return RedirectToAction(nameof(Details), category.ID);
        }


        // GET: Category/Delete/5
        public ActionResult Delete(int id)
        {
            ViewBag.Post = false;

            if (id == 0)
            {
                ViewBag.Succeeded = false;
                ViewBag.Message = "Deze categorie bestaat niet";

                return View();
            }

            return View();
        }

        // POST: Category/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            ViewBag.Post = true;

            categoryProcessor.Delete(id);

            ViewBag.Succeeded = true;
            return View();

        }
    }
}
