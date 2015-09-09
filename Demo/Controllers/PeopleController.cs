using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.OData;
using System.Web;
using System.Web.Http;
using Demo.DataSource;
using Demo.Models;
using Demo.Utils;
using System.Threading.Tasks;
using System.Net;
using System.Web.Http.Controllers;

namespace Demo.Controllers
{
    [EnableQuery]
    public class PeopleController : ODataController
    {
        // Just for testing
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);

            //if (Request.Headers.Accept.Count == 0 || )
            var query = HttpUtility.ParseQueryString(controllerContext.Request.RequestUri.Query);

            if (query.Get("$format") == "xml")
            {
                controllerContext.Request.Headers.Add("Accept", "application/atom+xml");
                controllerContext.Request.Headers.Add("Content-Type", "application/atom+xml");
            }
        }

        #region HTTP CRUD
        // HTTP POST - Create
        public async Task<IHttpActionResult> Post(Person person)
        {
            try
            {
                Logger.Instance.Info(string.Format("Start HTTP POST - People(person) is called.\r\nPerson is: {0}", person));

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                await Task.Run(() =>
                {
                    DemoDataSources.Instance.People.Add(person);
                });

                Logger.Instance.Info("Done HTTP POST - People(person)");
                return Created(person);
            }
            catch (Exception e)
            {
                Logger.Instance.Error(string.Format("HTTP POST - People({0})\r\n{1}", person, e.ToString()));
                return BadRequest(e.Message);
            }
            finally
            {
                // nothing else
            }
        }

        // HTTP GET all - Read
        public IHttpActionResult Get()
        {
            try
            {
                Logger.Instance.Info("Start HTTP GET - People is called");

                //if (Request.Headers.Accept.Count == 0 || )
                //var query = HttpUtility.ParseQueryString(Request.RequestUri.Query);

                //if(query.Get("$format") == "xml")
                //    Request.Headers.Add("Accept", "application/atom+xml");
                //else
                //    Request.Headers.Add("Accept", "application/json");


                IQueryable<Person> people = DemoDataSources.Instance.People.AsQueryable();
                Logger.Instance.Info("Done HTTP GET - People");
                return Ok(people);
            }
            catch (Exception e)
            {
                Logger.Instance.Error("HTTP GET - People.\r\n" + e.ToString());
                return BadRequest(e.Message);
            }
            finally
            {
                // nothing else
            }
        }

        // HTTP GET by key - Read
        public SingleResult<Person> Get([FromODataUri] string key)
        {
            try
            {
                Logger.Instance.Info(string.Format("Start HTTP GET - People(key) is called.\r\nKey is: {0}", key));

                var query = HttpUtility.ParseQueryString(Request.RequestUri.Query);
                if (query.Get("$format") == "xml")
                {
                    Request.Headers.Add("Accept", "application/atom+xml");
                    Request.Headers.Add("Content-Type", "application/atom+xml");
                }

                IQueryable<Person> result = DemoDataSources.Instance.People.Where(p => p.ID == key);
                Logger.Instance.Info(string.Format(string.Format("Done HTTP GET - People({0})", key)));
                return SingleResult.Create(result);
            }
            catch (Exception e)
            {
                Logger.Instance.Error(string.Format("HTTP GET - People({0})\r\n{1}", key, e.ToString()));
                return new SingleResult<Person>(new PeopleList()); // return empty value
            }
            finally
            {
                // nothing else
            }
        }

        // HTTP PATCH - Update
        public async Task<IHttpActionResult> Patch([FromODataUri] string key, Delta<Person> person)
        {
            try
            {
                Logger.Instance.Info(string.Format("Start HTTP PATCH - People(key, person) is called.\r\nKey is: {0}\r\nPerson is: {1}",key, person));

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var entity = await DemoDataSources.Instance.People.FindAsync(key);

                if (entity == null)
                {
                    return NotFound();
                }

                person.Patch(entity);

                try
                {
                    await Task.Run(() =>
                    {
                        Person toBeRemovedPerson = DemoDataSources.Instance.People.Where(p => p.ID == key).First();
                        DemoDataSources.Instance.People.Remove(toBeRemovedPerson);
                        DemoDataSources.Instance.People.Add(person.GetEntity());
                    });
                }
                catch
                {
                    if (null == DemoDataSources.Instance.People.Where(p => p.ID == key).First())
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                Logger.Instance.Info("Done HTTP PATCH - People(key, person)");
                return Updated(entity);
            }
            catch (Exception e)
            {
                Logger.Instance.Error(string.Format("HTTP PATCH - People({0}, {1})\r\n{2}", key, person, e.ToString()));
                return BadRequest(e.Message);
            }
            finally
            {
                // nothing else
            }
        }

        // HTTP PUT - Update
        public async Task<IHttpActionResult> Put([FromODataUri] string key, Person updatePerson)
        {
            try
            {
                Logger.Instance.Info(string.Format("Start HTTP PUT - People(key, person) is called.\r\nKey is: {0}\r\nPerson is: {1}", key, updatePerson));

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (key != updatePerson.ID)
                {
                    return BadRequest();
                }

                try
                {
                    await Task.Run(() =>
                    {
                        Person toBeRemovedPerson = DemoDataSources.Instance.People.Where(p => p.ID == key).First();
                        DemoDataSources.Instance.People.Remove(toBeRemovedPerson);

                        DemoDataSources.Instance.People.Add(updatePerson);
                    });
                }
                catch
                {
                    if (null == DemoDataSources.Instance.People.Where(p => p.ID == key).First())
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                Logger.Instance.Info("Done HTTP PUT - People(key, person)");
                return Updated(updatePerson);
            }
            catch (Exception e)
            {
                Logger.Instance.Error(string.Format("HTTP PUT - People({0}, {1})\r\n{2}", key, updatePerson, e.ToString()));
                return BadRequest(e.Message);
            }
            finally
            {
                // nothing else
            }
        }

        // HTTP DELETE - Delete
        public async Task<IHttpActionResult> Delete([FromODataUri] string key)
        {
            try
            {
                Logger.Instance.Info(string.Format("Start HTTP DELETE - People(key) is called.\r\nKey is: {0}", key));
                var product = await DemoDataSources.Instance.People.FindAsync(key);

                if (product == null)
                {
                    return NotFound();
                }

                await Task.Run(() =>
                {
                    Person toBeRemovedPerson = DemoDataSources.Instance.People.Where(p => p.ID == key).First();
                    DemoDataSources.Instance.People.Remove(toBeRemovedPerson);
                });

                Logger.Instance.Info("Done HTTP DELETE - People(key)");
                return StatusCode(HttpStatusCode.NoContent);
            }
            catch (Exception e)
            {
                Logger.Instance.Error(string.Format("HTTP DELETE - People({0})\r\n{1}", key, e.ToString()));
                return BadRequest(e.Message);
            }
            finally
            {
                // nothing else
            }
        }
        #endregion
    }
}