using System;
using System.Collections.Generic;
using System.Linq;

namespace PotterKata
{
    public class Bookshop
    {
        private PotterBookBasket _bookBasket;
        private List<Discount> _discounts;

        public Bookshop()
        {
            _bookBasket = new PotterBookBasket();
            _discounts = new List<Discount>()
            {
                {new Discount {NumberOfBooks = 2, PercentageOfFullPricePaid = 0.95, TotalCost = 15.2M} },
                {new Discount {NumberOfBooks = 3, PercentageOfFullPricePaid = 0.9, TotalCost = 21.6M} },
                {new Discount {NumberOfBooks = 4, PercentageOfFullPricePaid = 0.8, TotalCost = 25.6M} },
                {new Discount {NumberOfBooks = 5, PercentageOfFullPricePaid = 0.75, TotalCost = 30M} }
            };
        }

        public void AddToBasket(string bookTitle)
        {
            var bookToAdd = _bookBasket.BooksInBasket.Where(x => x.Title == bookTitle).FirstOrDefault();
            if(bookToAdd != null) bookToAdd.Quantity += 1;
        }

        public CheckoutBasket CheckoutBasket()
        {
            var distinctBooks = _bookBasket.BooksInBasket.Select(x => x.Quantity).Where(x => x > 0).ToArray();

            var checkoutBasket = new CheckoutBasket()
            {
                BooksPurchased = _bookBasket, UndiscountedBooks = distinctBooks, DiscountedPriceTotal = 0
            };

            var potentialCheckoutBaskets = GeneratePossibleCheckoutValues(new List<CheckoutBasket>() { checkoutBasket });

            var lowestPricedBasket = potentialCheckoutBaskets.OrderBy(x => x.DiscountedPriceTotal).First();

            return lowestPricedBasket;
        }

        private List<CheckoutBasket> GeneratePossibleCheckoutValues(List<CheckoutBasket> checkoutBaskets)
        {
            List<CheckoutBasket> processingBaskets = new List<CheckoutBasket>();
            bool undiscountedBooksRemaining = false;

            foreach (CheckoutBasket basket in checkoutBaskets)
            {
                var booksToProcess = basket.UndiscountedBooks;

                if (booksToProcess.Count() > 1)
                {
                    undiscountedBooksRemaining = true;
                    processingBaskets = IteratePossibleDiscounts(processingBaskets, basket, booksToProcess);
                }
                else
                {
                    basket.DiscountedPriceTotal  += CostOfAllRemainingBooks(basket.UndiscountedBooks);
                    processingBaskets.Add(basket);
                }
            }

            return undiscountedBooksRemaining == false ? processingBaskets : GeneratePossibleCheckoutValues(processingBaskets);
        }

        private List<CheckoutBasket> IteratePossibleDiscounts(List<CheckoutBasket> updatedBaskets, CheckoutBasket basket, int[] booksToProcess)
        {
            for (int booksInDiscount = 2; booksInDiscount <= booksToProcess.Count(); booksInDiscount++)
            {
                var discount = _discounts.Where(x => x.NumberOfBooks == booksInDiscount).FirstOrDefault();
                
                var discountsApplied = new List<Discount>(basket.DiscountsApplied);
                discountsApplied.Add(discount);

                var updatedBasket = new CheckoutBasket()
                {
                    BooksPurchased = basket.BooksPurchased,
                    UndiscountedBooks = UpdateUndiscountedBooksRecord(booksToProcess, booksInDiscount),
                    DiscountedPriceTotal = basket.DiscountedPriceTotal + discount.TotalCost,
                    DiscountsApplied = discountsApplied

                };

                updatedBaskets.Add(updatedBasket);
            }

            return updatedBaskets;
        }

        private int[] UpdateUndiscountedBooksRecord(int[] booksToProcess, int booksInDiscount)
        {
            var booksRemaining = OrderArrayByDescending(booksToProcess);
            var processedBooks = booksRemaining.Take(booksInDiscount).Select(x => x - 1);
            var unprocessedBooks = booksRemaining.Skip(booksInDiscount);

            return processedBooks.Concat(unprocessedBooks).Where(x => x > 0).ToArray();
        }

        private decimal CostOfAllRemainingBooks(int[] remainingBooks)
        {
            return remainingBooks.Count() == 0 ? 0 : remainingBooks[0] * 8;
        }

        private int[] OrderArrayByDescending(int[] array)
        {
            Array.Sort(array);
            Array.Reverse(array);
            return array;
        }
    }

    public class PotterBookBasket
    {
        public List<Book> BooksInBasket;

        public PotterBookBasket()
        {
            BooksInBasket = new List<Book>
            {
                new Book { Title = "first", BookSeriesNumber = 1, Quantity = 0 },
                new Book { Title = "second", BookSeriesNumber = 2, Quantity = 0 },
                new Book { Title = "third", BookSeriesNumber = 3, Quantity = 0 },
                new Book { Title = "fourth", BookSeriesNumber = 4, Quantity = 0 },
                new Book { Title = "fifth", BookSeriesNumber = 5, Quantity = 0 }
            };
        }
    }

    public class Discount
    {
        public int NumberOfBooks { get; set; }
        public double PercentageOfFullPricePaid { get; set; }
        public decimal TotalCost { get; set; }

    }

    public class Book
    {
        public string Title { get; set; }
        public int BookSeriesNumber { get; set; }
        public int Quantity { get; set; }
    }

    public class CheckoutBasket
    {
        public int[] UndiscountedBooks { get; set; }
        public decimal DiscountedPriceTotal { get; set; }
        public List<Discount> DiscountsApplied { get; set; }
        public PotterBookBasket BooksPurchased { get; set; }

        public CheckoutBasket()
        {
            DiscountsApplied = new List<Discount>();
        }
    }
}
