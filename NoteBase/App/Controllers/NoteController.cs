﻿using App.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NoteBaseLogicFactory;
using NoteBaseLogicInterface;
using NoteBaseLogicInterface.Models;
using System.Security.Cryptography;

namespace App.Controllers
{
    [Authorize]
    public class NoteController : Controller
    {
        private readonly IConfiguration _config;
        private readonly string connString;
        private readonly IPersonProcessor personProcessor;
        private readonly INoteProcessor noteProcessor;
        private readonly ICategoryProcessor categoryProcessor;
        private Person? person;

        public NoteController(IConfiguration configuration)
        {
            _config = configuration;
            connString = _config.GetConnectionString("NoteBaseConnString");
            personProcessor = ProcessorFactory.CreatePersonProcessor(connString);
            noteProcessor = ProcessorFactory.CreateNoteProcessor(connString);
            categoryProcessor = ProcessorFactory.CreateCategoryProcessor(connString);
        }

        // GET: Note/Details/5
        public ActionResult Details(int id)
        {
            Note note = noteProcessor.GetById(id);


            if (note.ID == 0)
            {
                ViewBag.Succeeded = false;
                ViewBag.Message = "Notitie niet gevonden";

                return View();
            }

            ViewBag.Succeeded = true;

            return View(new NoteModel(note));
        }

        // GET: Note/Create
        public ActionResult Create()
        {
            this.person = personProcessor.GetByEmail(User.Identity.Name);

            List<Category> categories = categoryProcessor.GetByPerson(this.person.ID);

            List<CategoryModel> categoryModels = new();
            foreach (Category category in categories)
            {
                categoryModels.Add(new(category));
            }

            ViewBag.CategoryList = categoryModels;
            ViewBag.Succeeded = true;

            return View();
        }

        // POST: Note/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            if (!noteProcessor.IsValidTitle(collection["Title"]))
            {
                ViewBag.Succeeded = false;
                ViewBag.Message = "Titel mag niet leeg zijn";

                return View();
            }
            if (!noteProcessor.IsTitleUnique(collection["Title"]))
            {
                ViewBag.Succeeded = false;
                ViewBag.Message = "Er bestaat al een notitie met deze titel";

                return View();
            }
            if (!noteProcessor.IsValidText(collection["Text"]))
            {
                ViewBag.Succeeded = false;
                ViewBag.Message = "Text mag niet leeg zijn";

                return View();
            }

            this.person = personProcessor.GetByEmail(User.Identity.Name);

            List<Category> categories = categoryProcessor.GetByPerson(person.ID);

            List<CategoryModel> categoryModels = new();
            foreach (Category category in categories)
            {
                categoryModels.Add(new(category));
            }
            ViewBag.CategoryList = categoryModels;

            Note note = noteProcessor.Create(collection["Title"], collection["Text"], Int32.Parse(collection["CategoryId"]), person.ID);

            if (note.ID == 0)
            {
                ViewBag.Succeeded = false;
                return View();
            }

            ViewBag.Succeeded = true;

            //for somereason when redirecting the details try to get the item before it has been added to database
            return RedirectToAction(nameof(Details), note.ID);
        }

        // GET: Note/Edit/5
        public ActionResult Edit(int id)
        {
            this.person = personProcessor.GetByEmail(User.Identity.Name);

            Note note = noteProcessor.GetById(id);

            List<Category> categories = categoryProcessor.GetByPerson(person.ID);

            List<CategoryModel> categoryModels = new();
            foreach (Category category in categories)
            {
                categoryModels.Add(new(category));
            }

            ViewBag.CategoryList = categoryModels;

            if (note.ID == 0)
            {
                ViewBag.Succeeded = false;
                ViewBag.Message = "Deze categorie bestaat niet";

                return View();
            }

            ViewBag.Succeeded = true;

            return View(new NoteModel(note));

        }

        // POST: Note/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            this.person = personProcessor.GetByEmail(User.Identity.Name);

            List<Category> categories = categoryProcessor.GetByPerson(person.ID);

            List<CategoryModel> categoryModels = new();
            foreach (Category category in categories)
            {
                categoryModels.Add(new(category));
            }

            ViewBag.CategoryList = categoryModels;

            if (!noteProcessor.IsValidTitle(collection["Title"]))
            {
                ViewBag.Succeeded = false;
                ViewBag.Message = "Titel mag niet leeg zijn";

                return View();
            }
            if (!noteProcessor.IsTitleUnique(collection["Title"], id))
            {
                ViewBag.Succeeded = false;
                ViewBag.Message = "Er bestaat al een notitie met deze titel";

                return View();
            }
            if (!noteProcessor.IsValidText(collection["Text"]))
            {
                ViewBag.Succeeded = false;
                ViewBag.Message = "Text mag niet leeg zijn";

                return View();
            }
            if (!noteProcessor.DoesNoteExits(id))
            {
                ViewBag.Succeeded = false;
                ViewBag.Message = "De notitie met dit id bestaat niet";

                return View();
            }

            //retrieve note first to get the tags
            Note note = noteProcessor.GetById(id);

            note = noteProcessor.Update(id, collection["Title"], collection["Text"], Int32.Parse(collection["CategoryId"]), person.ID, note.tagList);

            if (note.ID == 0)
            {
                ViewBag.Succeeded = false;
                ViewBag.Message = "";

                return View();
            }

            ViewBag.Succeeded = true;

            //diffrent redirect options?
            return RedirectToAction(nameof(Details), note.ID);

        }

        // GET: Note/Delete/5
        public ActionResult Delete(int id)
        {
            ViewBag.Post = false;

            if (id == 0)
            {
                ViewBag.Succeeded = false;
                ViewBag.Message = "Deze notitiie bestaat niet";

                return View();
            }

            return View();
        }

        // POST: Note/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            ViewBag.Post = true;

            this.person = personProcessor.GetByEmail(User.Identity.Name);

            Note note = noteProcessor.GetById(id);

            noteProcessor.Delete(note.ID, note.tagList, person.ID);

            ViewBag.Succeeded = true;

            return View();
        }
    }
}
