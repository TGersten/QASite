using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QASite.Data
{
    public class QuestionsRepository
    {
        private readonly string _connectionString;

        public QuestionsRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        private Tag GetTag(string name)
        {
            using var ctx = new QuestionsDataContext(_connectionString);
            return ctx.Tags.FirstOrDefault(t => t.Name == name);
        }

        private int AddTag(string name)
        {
            using var ctx = new QuestionsDataContext(_connectionString);
            var tag = new Tag { Name = name };
            ctx.Tags.Add(tag);
            ctx.SaveChanges();
            return tag.Id;
        }

        public List<Question> GetQuestionsForTag(string name)
        {
            using var ctx = new QuestionsDataContext(_connectionString);
            return ctx.Questions
                    .Where(c => c.QuestionsTags.Any(t => t.Tag.Name == name))
                    .Include(q => q.QuestionsTags)
                    .ThenInclude(qt => qt.Tag)
                    .ToList();
        }


        public void AddQuestion(Question question, List<string> tags)
        {
            using var ctx = new QuestionsDataContext(_connectionString);
            ctx.Questions.Add(question);
            ctx.SaveChanges();
            foreach (string tag in tags)
            {
                Tag t = GetTag(tag);
                int tagId;
                if (t == null)
                {
                    tagId = AddTag(tag);
                }
                else
                {
                    tagId = t.Id;
                }
                ctx.QuestionsTags.Add(new QuestionsTags
                {
                    QuestionId = question.Id,
                    TagId = tagId
                });
            }

            ctx.SaveChanges();
        }


        public List<Question> GetAllQuestions()
        {
            using var ctx = new QuestionsDataContext(_connectionString);
            return ctx.Questions
                .Include (q=> q.Answers)
                .Include (q=> q.QuestionsTags)
                .ThenInclude(qt=> qt.Tag)
                .OrderByDescending(q=>q.DatePosted).ToList();
        }

        public Question GetQuestionForId(int id)
        {
            using var ctx = new QuestionsDataContext(_connectionString);
            return ctx.Questions.Where(q=> q.Id== id)
                .Include(q=> q.User)
                .Include(q=>q.Answers)
                .ThenInclude(a=>a.User)
                .Include(q=> q.QuestionsTags)
                .ThenInclude(qt=>qt.Tag)
                .FirstOrDefault(q => q.Id == id);
        }

        public void AddAnswer(Answer answer)
        {
            using var ctx = new QuestionsDataContext(_connectionString);
            ctx.Answers.Add(answer);
            ctx.SaveChanges();

        }
    }
}

