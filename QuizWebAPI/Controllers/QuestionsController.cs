using QuizWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace QuizWebAPI.Controllers
{
    public class QuestionsController : ApiController
    {
        [HttpGet]
        [Route("api/Questions")]
        public HttpResponseMessage GetQuesitons(Question model)
        {
            using (DBModel db = new DBModel())
            {
                var Qns = db.Questions
                .Select(x => new { QnID = x.QnID, Qn = x.Qn, ImageName = x.ImageName, option1 = x.Option1, option2 = x.Option2, option3 = x.Option3, option4 = x.Option4 })
                .OrderBy(y => Guid.NewGuid())
                .Take(10)
                .ToArray();

                var updated = Qns.AsEnumerable()
                   .Select(x => new
                   {
                       QnID = x.QnID,
                       Qn = x.Qn,
                       ImageName = x.ImageName,
                       options = new String[] { x.option1, x.option2, x.option3, x.option4 }
                   }).ToList();

                return this.Request.CreateResponse(HttpStatusCode.OK, updated);
            }
        }


        [HttpGet]
        [Route("api/Answers")]

        public HttpResponseMessage GetAnswers(int[] qIDs)
        {
            using (DBModel db = new DBModel())
            {
                var result = db.Questions.AsEnumerable()
                .Where(y => qIDs.Contains(y.QnID))
                .OrderBy(x => { return Array.IndexOf(qIDs, x.QnID); })
                .Select(z => z.Answer)
                .ToArray();
                return this.Request.CreateResponse(HttpStatusCode.OK, result);
            }
        }
    }
}
