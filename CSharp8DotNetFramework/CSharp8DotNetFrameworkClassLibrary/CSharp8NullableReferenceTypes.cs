using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharp8DotNetFramework
{
    // Nullable reference types tutorial: https://docs.microsoft.com/en-us/dotnet/csharp/tutorials/nullable-reference-types
    internal class CSharp8NullableReferenceTypes
    {
        static CSharp8NullableReferenceTypes()
        {
            var x = new SurveyRun();
        }

        public enum QuestionType
        {
            YesNo,
            Number,
            Text
        }

        public class SurveyQuestion
        {
            public SurveyQuestion(QuestionType typeOfQuestion, string text) =>
                (TypeOfQuestion, QuestionText) = (typeOfQuestion, text);

            public string QuestionText { get; }

            public QuestionType TypeOfQuestion { get; }
        }

        public class SurveyResponse

        {
            // <SnippetRandom>
            private static readonly Random randomGenerator = new Random();

            // <SnippetAnswerSurvey>
            private Dictionary<int, string>? surveyResponses;

            public SurveyResponse(int id) => Id = id;

            // <SnippetSurveyStatus>
            public bool AnsweredSurvey => surveyResponses != null;

            public int Id { get; }

            public static SurveyResponse GetRandomId() => new SurveyResponse(randomGenerator.Next());

            // </SnippetRandom>
            public string Answer(int index)
            {
                string? answer = null;
                surveyResponses?.TryGetValue(index, out answer);
                return answer ?? "No answer";
            }

            // </SnippetSurveyStatus>
            public bool AnswerSurvey(IEnumerable<SurveyQuestion> questions)
            {
                if (ConsentToSurvey())
                {
                    surveyResponses = new Dictionary<int, string>();
                    int index = 0;
                    foreach (var question in questions)
                    {
                        var answer = GenerateAnswer(question);
                        if (answer != null)
                        {
                            surveyResponses.Add(index, answer);
                        }
                        index++;
                    }
                }
                return surveyResponses != null;
            }

            private static bool ConsentToSurvey() => randomGenerator.Next(0, 2) == 1;

            private static string? GenerateAnswer(SurveyQuestion question)
            {
                switch (question.TypeOfQuestion)
                {
                case QuestionType.YesNo:
                    int n = randomGenerator.Next(-1, 2);
                    return (n == -1) ? default : (n == 0) ? "No" : "Yes";

                case QuestionType.Number:
                    n = randomGenerator.Next(-30, 101);
                    return (n < 0) ? default : n.ToString();

                case QuestionType.Text:
                default:
                    switch (randomGenerator.Next(0, 5))
                    {
                    case 0:
                        return default;

                    case 1:
                        return "Red";

                    case 2:
                        return "Green";

                    case 3:
                        return "Blue";
                    }
                    return "Red. No, Green. Wait.. Blue... AAARGGGGGHHH!";
                }
            }

            // </SnippetAnswerSurvey>
        }

        public class SurveyRun
        {
            // <SnippetPerformSurvey>
            private List<SurveyResponse>? respondents;

            private List<SurveyQuestion> surveyQuestions = new List<SurveyQuestion>();

            // <SnippetRunReport>
            public IEnumerable<SurveyResponse> AllParticipants => (respondents ?? Enumerable.Empty<SurveyResponse>());

            public ICollection<SurveyQuestion> Questions => surveyQuestions;

            public void AddQuestion(QuestionType type, string question) =>
                AddQuestion(new SurveyQuestion(type, question));

            // </SnippetRunReport>
            public void AddQuestion(SurveyQuestion surveyQuestion) => surveyQuestions.Add(surveyQuestion);

            public SurveyQuestion GetQuestion(int index) => surveyQuestions[index];

            public void PerformSurvey(int numberOfRespondents)
            {
                int repondentsConsenting = 0;
                respondents = new List<SurveyResponse>();
                while (repondentsConsenting < numberOfRespondents)
                {
                    var respondent = SurveyResponse.GetRandomId();
                    if (respondent.AnswerSurvey(surveyQuestions))
                        repondentsConsenting++;
                    respondents.Add(respondent);
                }
            }

            // </SnippetPerformSurvey>
        }
    }
}