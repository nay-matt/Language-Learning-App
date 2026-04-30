namespace Language_Learning_App.Services
{
    using Language_Learning_App.Models;
    using System;

    public class SRSService
    {
        public void CalculateNextReview(FlashcardReview review, int quality)
        {
            if (quality >= 2)
            {
                if (review.TimesReviewed == 0)
                {
                    review.Interval = 1;
                }
                else if (review.TimesReviewed == 1)
                {
                    review.Interval = 6;
                }
                else
                {
                    double multiplier = (quality == 2) ? 1.2 : review.EaseFactor;
                    review.Interval = (int)Math.Ceiling(review.Interval * multiplier);
                }

                review.TimesReviewed++;
            }
            else
            {
                review.Interval = 1;
                review.TimesReviewed = 0;
                review.EaseFactor -= 0.2;
            }

            double factorChange = 0.1 - (5 - quality) * (0.08 + (5 - quality) * 0.02);
            review.EaseFactor += factorChange;

            if (review.EaseFactor < 1.3) review.EaseFactor = 1.3;
            if (review.EaseFactor > 5.0) review.EaseFactor = 5.0;

            review.NextReviewDate = DateTime.Now.AddDays(review.Interval);
            review.LastReviewed = DateTime.Now;
        }
    }
}