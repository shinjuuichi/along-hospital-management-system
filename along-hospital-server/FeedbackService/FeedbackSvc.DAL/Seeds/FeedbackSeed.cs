using FeedbackSvc.DAL.Enums;
using FeedbackSvc.DAL.Models;
using Microsoft.EntityFrameworkCore;
using SharedLibrary.Base.Data;

namespace FeedbackSvc.DAL.Seeds
{
    public class FeedbackSeed : ISeedBuilder
    {
        public int Priority => 1;

        public ModelBuilder Seed(ModelBuilder modelBuilder)
        {
            var seedCreationDate = new DateTime(2025, 01, 01, 0, 0, 0, DateTimeKind.Utc);
            var patientIds = new[] { 3, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61, 62, 63 };
            var feedbacks = new List<Feedback>();

            this.AddFeedbackBatch(feedbacks, 1, 1, seedCreationDate, patientIds,
            [
                ("Great medicine, really helped with my symptoms and had no side effects.", 5, FeedbackTypeEnum.Positive, FeedbackReplyStatusEnum.Replied),
                ("Felt better after a few days of use. Works as described.", 4, FeedbackTypeEnum.Positive, FeedbackReplyStatusEnum.WaitingStaff),
                ("The medicine was okay, but caused mild dizziness after taking it.", 3, FeedbackTypeEnum.Neutral, FeedbackReplyStatusEnum.Replied),
                ("Easy to swallow and reduced my fever quickly.", 5, FeedbackTypeEnum.Positive, FeedbackReplyStatusEnum.Replied),
                ("Relieved my headache well, but the tablet tasted a little bitter.", 4, FeedbackTypeEnum.Positive, FeedbackReplyStatusEnum.WaitingStaff),
                ("Worked for my muscle pain and I did not notice stomach discomfort.", 5, FeedbackTypeEnum.Positive, FeedbackReplyStatusEnum.Replied),
                ("Packaging was neat and the medicine arrived with a long expiry date.", 4, FeedbackTypeEnum.Positive, FeedbackReplyStatusEnum.WaitingStaff),
                ("Helped lower my fever the same evening and I slept much better.", 5, FeedbackTypeEnum.Positive, FeedbackReplyStatusEnum.Replied),
                ("The effect wore off sooner than I expected, but it still helped.", 3, FeedbackTypeEnum.Neutral, FeedbackReplyStatusEnum.WaitingStaff),
                ("Reliable pain relief and easy to keep at home for common use.", 5, FeedbackTypeEnum.Positive, FeedbackReplyStatusEnum.Replied),
                ("It reduced body aches, though I needed to take another dose later.", 3, FeedbackTypeEnum.Neutral, FeedbackReplyStatusEnum.WaitingStaff),
                ("Very useful for family use and the instructions were easy to follow.", 5, FeedbackTypeEnum.Positive, FeedbackReplyStatusEnum.Replied),
                ("The medicine worked, but delivery took longer than expected.", 4, FeedbackTypeEnum.Positive, FeedbackReplyStatusEnum.WaitingStaff),
                ("Good quality product and the fever relief was stable for several hours.", 5, FeedbackTypeEnum.Positive, FeedbackReplyStatusEnum.Replied),
                ("My headache improved, but the result was only moderate for me.", 3, FeedbackTypeEnum.Neutral, FeedbackReplyStatusEnum.WaitingStaff),
                ("I used it after vaccination and the discomfort improved quickly.", 5, FeedbackTypeEnum.Positive, FeedbackReplyStatusEnum.Replied),
                ("Did not notice much improvement after the first dose, maybe needs more time.", 2, FeedbackTypeEnum.Negative, FeedbackReplyStatusEnum.WaitingStaff),
                ("Pain relief was reliable and the medicine did not upset my stomach.", 5, FeedbackTypeEnum.Positive, FeedbackReplyStatusEnum.Replied),
                ("The tablet size was fine, but I hoped it would act a bit faster.", 4, FeedbackTypeEnum.Positive, FeedbackReplyStatusEnum.WaitingStaff),
                ("Helpful product overall, but the instructions could be clearer for first-time users.", 3, FeedbackTypeEnum.Neutral, FeedbackReplyStatusEnum.Replied),
                ("Worked well for toothache and the relief lasted through the night.", 5, FeedbackTypeEnum.Positive, FeedbackReplyStatusEnum.WaitingStaff),
                ("The result was decent, though I still needed rest to recover fully.", 4, FeedbackTypeEnum.Positive, FeedbackReplyStatusEnum.WaitingStaff),
                ("A practical medicine to keep in the cabinet for sudden fever.", 5, FeedbackTypeEnum.Positive, FeedbackReplyStatusEnum.WaitingStaff),
                ("Helped my sore throat discomfort a little, but not as much as expected.", 3, FeedbackTypeEnum.Neutral, FeedbackReplyStatusEnum.WaitingStaff),
                ("Easy to use, affordable, and the quality felt trustworthy.", 5, FeedbackTypeEnum.Positive, FeedbackReplyStatusEnum.WaitingStaff),
                ("The medicine worked for me, although I felt slightly sleepy afterward.", 3, FeedbackTypeEnum.Neutral, FeedbackReplyStatusEnum.WaitingStaff),
                ("A dependable choice for common pain and fever in my family.", 5, FeedbackTypeEnum.Positive, FeedbackReplyStatusEnum.WaitingStaff),
                ("Took effect after about an hour and eased my headache enough to work.", 4, FeedbackTypeEnum.Positive, FeedbackReplyStatusEnum.WaitingStaff),
                ("No major complaints, but the effect felt average compared with other brands.", 3, FeedbackTypeEnum.Neutral, FeedbackReplyStatusEnum.WaitingStaff),
                ("Very satisfied with the outcome and would purchase it again.", 5, FeedbackTypeEnum.Positive, FeedbackReplyStatusEnum.WaitingStaff)
            ]);

            this.AddFeedbackBatch(feedbacks, 31, 3, seedCreationDate.AddDays(30), patientIds,
            [
                ("Vitamin tablets had a pleasant taste and were easy to take every morning.", 5, FeedbackTypeEnum.Positive, FeedbackReplyStatusEnum.WaitingStaff),
                ("I felt less tired after using it regularly for a week.", 4, FeedbackTypeEnum.Positive, FeedbackReplyStatusEnum.WaitingStaff),
                ("The tablets were a bit large, but the overall quality felt good.", 4, FeedbackTypeEnum.Positive, FeedbackReplyStatusEnum.WaitingStaff),
                ("I did not notice much improvement after two uses, maybe it needs more time.", 2, FeedbackTypeEnum.Negative, FeedbackReplyStatusEnum.WaitingStaff),
                ("Good supplement to keep at home during weather changes.", 5, FeedbackTypeEnum.Positive, FeedbackReplyStatusEnum.WaitingStaff),
                ("The flavor was acceptable and the product looked genuine.", 4, FeedbackTypeEnum.Positive, FeedbackReplyStatusEnum.WaitingStaff),
                ("Helped me recover faster from a mild cold when combined with rest.", 5, FeedbackTypeEnum.Positive, FeedbackReplyStatusEnum.WaitingStaff),
                ("I expected more energy, but the effect was only moderate.", 3, FeedbackTypeEnum.Neutral, FeedbackReplyStatusEnum.WaitingStaff),
                ("Easy to add to my daily routine and did not upset my stomach.", 5, FeedbackTypeEnum.Positive, FeedbackReplyStatusEnum.WaitingStaff),
                ("The package arrived in good condition and the seal was intact.", 4, FeedbackTypeEnum.Positive, FeedbackReplyStatusEnum.WaitingStaff),
                ("Useful product, though the price is a little higher than similar options.", 4, FeedbackTypeEnum.Positive, FeedbackReplyStatusEnum.WaitingStaff),
                ("I felt more comfortable during flu season after taking it daily.", 5, FeedbackTypeEnum.Positive, FeedbackReplyStatusEnum.WaitingStaff),
                ("The result was acceptable, but I hoped the tablets would dissolve faster.", 3, FeedbackTypeEnum.Neutral, FeedbackReplyStatusEnum.WaitingStaff),
                ("Taste was fine and there was no strange aftertaste.", 5, FeedbackTypeEnum.Positive, FeedbackReplyStatusEnum.WaitingStaff),
                ("My throat irritation improved slightly after a few days.", 4, FeedbackTypeEnum.Positive, FeedbackReplyStatusEnum.WaitingStaff),
                ("The product looked clean and trustworthy from packaging to tablet quality.", 5, FeedbackTypeEnum.Positive, FeedbackReplyStatusEnum.WaitingStaff),
                ("It worked for me, but not as noticeably as I expected.", 3, FeedbackTypeEnum.Neutral, FeedbackReplyStatusEnum.WaitingStaff),
                ("A convenient vitamin choice for busy mornings.", 5, FeedbackTypeEnum.Positive, FeedbackReplyStatusEnum.WaitingStaff),
                ("The tablets were easy to swallow and did not cause nausea.", 5, FeedbackTypeEnum.Positive, FeedbackReplyStatusEnum.WaitingStaff),
                ("I used it during recovery and felt my appetite improve a little.", 4, FeedbackTypeEnum.Positive, FeedbackReplyStatusEnum.WaitingStaff),
                ("No major issues, but the effect felt gradual rather than immediate.", 3, FeedbackTypeEnum.Neutral, FeedbackReplyStatusEnum.WaitingStaff),
                ("My family also used it and everyone found it easy to take.", 5, FeedbackTypeEnum.Positive, FeedbackReplyStatusEnum.WaitingStaff),
                ("The quality seems stable and the brand feels reliable.", 5, FeedbackTypeEnum.Positive, FeedbackReplyStatusEnum.WaitingStaff),
                ("I had mild stomach warmth once, but it went away quickly.", 3, FeedbackTypeEnum.Neutral, FeedbackReplyStatusEnum.WaitingStaff),
                ("Helpful as a daily supplement, especially when I do not eat enough fruit.", 5, FeedbackTypeEnum.Positive, FeedbackReplyStatusEnum.WaitingStaff),
                ("The bottle was easy to open and store in my bag.", 4, FeedbackTypeEnum.Positive, FeedbackReplyStatusEnum.WaitingStaff),
                ("I bought it for immune support and I am generally satisfied.", 4, FeedbackTypeEnum.Positive, FeedbackReplyStatusEnum.WaitingStaff),
                ("The benefit was there, but the pace felt slower than expected.", 3, FeedbackTypeEnum.Neutral, FeedbackReplyStatusEnum.WaitingStaff),
                ("Clean product, good taste, and no unpleasant side effects.", 5, FeedbackTypeEnum.Positive, FeedbackReplyStatusEnum.WaitingStaff),
                ("Overall a good supplement and I would consider buying it again.", 5, FeedbackTypeEnum.Positive, FeedbackReplyStatusEnum.WaitingStaff)
            ]);

            this.AddFeedbackBatch(feedbacks, 61, 5, seedCreationDate.AddDays(60), patientIds,
            [
                ("I used it for dry eyes and the improvement was noticeable after two days.", 5, FeedbackTypeEnum.Positive, FeedbackReplyStatusEnum.WaitingStaff),
                ("The eye drops were gentle and did not sting like some other brands.", 5, FeedbackTypeEnum.Positive, FeedbackReplyStatusEnum.WaitingStaff),
                ("My eyes felt less irritated, but I still needed to use the drops several times a day.", 4, FeedbackTypeEnum.Positive, FeedbackReplyStatusEnum.WaitingStaff),
                ("The bottle was easy to use, though the drop size felt a little large.", 4, FeedbackTypeEnum.Positive, FeedbackReplyStatusEnum.WaitingStaff),
                ("Helped with screen-related dryness during office hours.", 5, FeedbackTypeEnum.Positive, FeedbackReplyStatusEnum.WaitingStaff),
                ("Relief came quickly and there was no burning sensation.", 5, FeedbackTypeEnum.Positive, FeedbackReplyStatusEnum.WaitingStaff),
                ("The formula felt smooth, but I hoped the moisture would last longer.", 4, FeedbackTypeEnum.Positive, FeedbackReplyStatusEnum.WaitingStaff),
                ("Good product for daily use when my eyes feel tired.", 5, FeedbackTypeEnum.Positive, FeedbackReplyStatusEnum.WaitingStaff),
                ("The cooling feeling was comfortable and not too strong.", 4, FeedbackTypeEnum.Positive, FeedbackReplyStatusEnum.WaitingStaff),
                ("Easy to carry in my bag and use while traveling.", 5, FeedbackTypeEnum.Positive, FeedbackReplyStatusEnum.WaitingStaff),
                ("My eye discomfort improved, though not completely on the first day.", 4, FeedbackTypeEnum.Positive, FeedbackReplyStatusEnum.WaitingStaff),
                ("No irritation after use and the quality felt very stable.", 5, FeedbackTypeEnum.Positive, FeedbackReplyStatusEnum.WaitingStaff),
                ("Helpful after long hours with contact lenses.", 5, FeedbackTypeEnum.Positive, FeedbackReplyStatusEnum.WaitingStaff),
                ("The bottle design is practical and easy to control.", 4, FeedbackTypeEnum.Positive, FeedbackReplyStatusEnum.WaitingStaff),
                ("My eyes stopped feeling gritty after regular use.", 5, FeedbackTypeEnum.Positive, FeedbackReplyStatusEnum.WaitingStaff),
                ("A good choice for mild dryness, though severe cases may need more support.", 4, FeedbackTypeEnum.Positive, FeedbackReplyStatusEnum.WaitingStaff),
                ("The drops were clean and easy to apply even for first-time users.", 5, FeedbackTypeEnum.Positive, FeedbackReplyStatusEnum.WaitingStaff),
                ("I noticed clearer vision comfort while reading for long periods.", 5, FeedbackTypeEnum.Positive, FeedbackReplyStatusEnum.WaitingStaff),
                ("The effect was decent, but I needed to reapply by afternoon.", 3, FeedbackTypeEnum.Neutral, FeedbackReplyStatusEnum.WaitingStaff),
                ("Very comfortable product and suitable for office workers.", 5, FeedbackTypeEnum.Positive, FeedbackReplyStatusEnum.WaitingStaff),
                ("No strong smell, no sting, and the bottle stayed leak-free.", 5, FeedbackTypeEnum.Positive, FeedbackReplyStatusEnum.WaitingStaff),
                ("Worked well in air-conditioned rooms where my eyes usually dry out.", 5, FeedbackTypeEnum.Positive, FeedbackReplyStatusEnum.WaitingStaff),
                ("The relief was moderate but still worth using.", 4, FeedbackTypeEnum.Positive, FeedbackReplyStatusEnum.WaitingStaff),
                ("My eye redness reduced a little after a few applications.", 4, FeedbackTypeEnum.Positive, FeedbackReplyStatusEnum.WaitingStaff),
                ("Very handy for travel and did not take much space.", 5, FeedbackTypeEnum.Positive, FeedbackReplyStatusEnum.WaitingStaff),
                ("I liked the gentle formula and would buy it again.", 5, FeedbackTypeEnum.Positive, FeedbackReplyStatusEnum.WaitingStaff),
                ("Good daily support for dry eye symptoms after computer work.", 5, FeedbackTypeEnum.Positive, FeedbackReplyStatusEnum.WaitingStaff),
                ("The product quality was fine, but the cap was a little tight to open.", 4, FeedbackTypeEnum.Positive, FeedbackReplyStatusEnum.WaitingStaff),
                ("A reliable option when my eyes feel strained and tired.", 5, FeedbackTypeEnum.Positive, FeedbackReplyStatusEnum.WaitingStaff),
                ("Overall very satisfied because it eased discomfort without irritation.", 5, FeedbackTypeEnum.Positive, FeedbackReplyStatusEnum.WaitingStaff)
            ]);

            this.AddFeedbackBatch(feedbacks, 91, 8, seedCreationDate.AddDays(90), patientIds,
            [
                ("The powder dissolved quickly in water and was convenient during stomach illness.", 5, FeedbackTypeEnum.Positive, FeedbackReplyStatusEnum.WaitingStaff),
                ("The taste was hard to get used to, but it helped me recover from dehydration.", 4, FeedbackTypeEnum.Positive, FeedbackReplyStatusEnum.WaitingStaff),
                ("Very useful to keep at home. My child recovered well after using it as directed.", 5, FeedbackTypeEnum.Positive, FeedbackReplyStatusEnum.WaitingStaff),
                ("Packaging was secure and the product matched the description on the page.", 4, FeedbackTypeEnum.Positive, FeedbackReplyStatusEnum.WaitingStaff),
                ("Worked well after stomach upset and helped me feel steadier.", 5, FeedbackTypeEnum.Positive, FeedbackReplyStatusEnum.WaitingStaff),
                ("The salty taste was strong, but the result was worth it.", 4, FeedbackTypeEnum.Positive, FeedbackReplyStatusEnum.WaitingStaff),
                ("Easy to prepare and useful for emergencies at home.", 5, FeedbackTypeEnum.Positive, FeedbackReplyStatusEnum.WaitingStaff),
                ("I noticed improvement after the first glass and felt less weak.", 5, FeedbackTypeEnum.Positive, FeedbackReplyStatusEnum.WaitingStaff),
                ("Helpful product overall, though the instructions could be clearer for first-time users.", 3, FeedbackTypeEnum.Neutral, FeedbackReplyStatusEnum.WaitingStaff),
                ("Good to keep in the cabinet for children and adults alike.", 5, FeedbackTypeEnum.Positive, FeedbackReplyStatusEnum.WaitingStaff),
                ("The sachet was easy to tear and mix with water.", 5, FeedbackTypeEnum.Positive, FeedbackReplyStatusEnum.WaitingStaff),
                ("I recovered from diarrhea more comfortably after using it.", 5, FeedbackTypeEnum.Positive, FeedbackReplyStatusEnum.WaitingStaff),
                ("The flavor is not great, but the hydration support is effective.", 4, FeedbackTypeEnum.Positive, FeedbackReplyStatusEnum.WaitingStaff),
                ("Convenient travel item and useful during hot days.", 5, FeedbackTypeEnum.Positive, FeedbackReplyStatusEnum.WaitingStaff),
                ("My child accepted the taste better than I expected.", 4, FeedbackTypeEnum.Positive, FeedbackReplyStatusEnum.WaitingStaff),
                ("Worked reliably and the quality felt consistent.", 5, FeedbackTypeEnum.Positive, FeedbackReplyStatusEnum.WaitingStaff),
                ("I would like a milder taste, but the product itself is helpful.", 3, FeedbackTypeEnum.Neutral, FeedbackReplyStatusEnum.WaitingStaff),
                ("Very practical medicine to keep ready for dehydration risks.", 5, FeedbackTypeEnum.Positive, FeedbackReplyStatusEnum.WaitingStaff),
                ("The instructions were simple enough and preparation took less than a minute.", 5, FeedbackTypeEnum.Positive, FeedbackReplyStatusEnum.WaitingStaff),
                ("It helped after vomiting, although I needed to sip it slowly.", 4, FeedbackTypeEnum.Positive, FeedbackReplyStatusEnum.WaitingStaff),
                ("Affordable, effective, and easy to store at home.", 5, FeedbackTypeEnum.Positive, FeedbackReplyStatusEnum.WaitingStaff),
                ("The taste was slightly unpleasant, but I still finished the dose.", 3, FeedbackTypeEnum.Neutral, FeedbackReplyStatusEnum.WaitingStaff),
                ("Useful during fever and stomach illness when hydration is important.", 5, FeedbackTypeEnum.Positive, FeedbackReplyStatusEnum.WaitingStaff),
                ("The product quality seemed good and the sachets were well sealed.", 5, FeedbackTypeEnum.Positive, FeedbackReplyStatusEnum.WaitingStaff),
                ("My energy improved after rehydrating with this product.", 5, FeedbackTypeEnum.Positive, FeedbackReplyStatusEnum.WaitingStaff),
                ("It worked as expected, though I would prefer smaller sachets for children.", 4, FeedbackTypeEnum.Positive, FeedbackReplyStatusEnum.WaitingStaff),
                ("A dependable product and easy to recommend for home use.", 5, FeedbackTypeEnum.Positive, FeedbackReplyStatusEnum.WaitingStaff),
                ("The result was good, but the flavor may not suit everyone.", 3, FeedbackTypeEnum.Neutral, FeedbackReplyStatusEnum.WaitingStaff),
                ("I keep this product in the house because it has been useful more than once.", 5, FeedbackTypeEnum.Positive, FeedbackReplyStatusEnum.WaitingStaff),
                ("Overall satisfied because it helped quickly during dehydration.", 5, FeedbackTypeEnum.Positive, FeedbackReplyStatusEnum.WaitingStaff)
            ]);

            modelBuilder.Entity<Feedback>().HasData(feedbacks.ToArray());

            return modelBuilder;
        }

        private void AddFeedbackBatch(
            List<Feedback> feedbacks,
            int startId,
            int medicineId,
            DateTime startDate,
            int[] patientIds,
            IEnumerable<(string Content, double Rating, FeedbackTypeEnum FeedbackType, FeedbackReplyStatusEnum FeedbackReplyStatus)> items)
        {
            var index = 0;

            foreach (var item in items)
            {
                feedbacks.Add(new Feedback
                {
                    Id = startId + index,
                    Content = item.Content,
                    Rating = item.Rating,
                    MedicineId = medicineId,
                    FeedbackTypeEnum = item.FeedbackType,
                    FeedbackReplyStatus = item.FeedbackReplyStatus,
                    CreationDate = startDate.AddDays(index),
                    CreatedBy = patientIds[index % patientIds.Length]
                });

                index++;
            }
        }
    }
}
