using System.Collections.Generic;
using PotterKata;
using Xunit;

namespace PotterKataTests
{
    public class BookshopTests
    {
        private const string _firstBook = "first";
        private const string _secondBook = "second";
        private const string _thirdBook = "third";
        private const string _fourthBook = "fourth";
        private const string _fifthBook = "fifth";

        [Fact]
        public void Checkout_WhenNoBooksArePurchased_ReturnsPriceOf0()
        {
            var bookshop = new Bookshop();

            bookshop.AddToBasket("");
            var checkoutTotal = bookshop.CheckoutBasket();

            var expected = 0;
            Assert.Equal(expected, checkoutTotal.DiscountedPriceTotal); 
        }

        [InlineData("first")]
        [InlineData("second")]
        [InlineData("third")]
        [InlineData("fourth")]
        [InlineData("fifth")]
        [Theory]
        public void Checkout_WhenSinglePotterBookIsPurchased_ReturnsPriceOf8(string bookNumber)
        {
            var bookshop = new Bookshop();

            bookshop.AddToBasket(bookNumber);
            var checkoutTotal = bookshop.CheckoutBasket();

            var expected = 8M;
            Assert.Equal(expected, checkoutTotal.DiscountedPriceTotal);
        }

        [Fact]
        public void Checkout_WhenTwoOfTheSameBooksArePurchased_ReturnsPriceOf16()
        {
            var bookshop = new Bookshop();

            bookshop.AddToBasket("first");
            bookshop.AddToBasket("first");
            var checkoutTotal = bookshop.CheckoutBasket();

            var expected = 16M;
            Assert.Equal(expected, checkoutTotal.DiscountedPriceTotal);
        }

        [InlineData(new string[] { _firstBook, _secondBook}, 15.2)]
        [InlineData(new string[] { _firstBook, _secondBook, _thirdBook }, 21.6)]
        [InlineData(new string[] { _firstBook, _secondBook, _thirdBook, _fourthBook }, 25.6)]
        [InlineData(new string[] { _firstBook, _secondBook, _thirdBook, _fourthBook, _fifthBook }, 30)]
        [Theory]
        public void Checkout_WhenOnlyDifferentBooksArePurchased_ReturnsCorrectDiscountedPrice(string[] purchasedBooks, decimal expected)
        {
            var bookshop = new Bookshop();
            foreach(string book in purchasedBooks)
            {
                bookshop.AddToBasket(book);
            };
            var checkoutTotal = bookshop.CheckoutBasket();

            Assert.Equal(expected, checkoutTotal.DiscountedPriceTotal);
        }


        [Fact]
        public void Checkout_WhenTwoOfEveryBooksIsPurchased_ReturnsDiscountedPrice()
        {
            var bookshop = new Bookshop();

            bookshop.AddToBasket("first");
            bookshop.AddToBasket("second");
            bookshop.AddToBasket("third");
            bookshop.AddToBasket("fourth");
            bookshop.AddToBasket("fifth");
            bookshop.AddToBasket("first");
            bookshop.AddToBasket("second");
            bookshop.AddToBasket("third");
            bookshop.AddToBasket("fourth");
            bookshop.AddToBasket("fifth");
            var checkoutTotal = bookshop.CheckoutBasket();

            var expected = 60M;
            Assert.Equal(expected, checkoutTotal.DiscountedPriceTotal);
        }

        [InlineData(new string[] { _firstBook, _secondBook, _thirdBook, _fourthBook, _fifthBook, _firstBook, _secondBook, _thirdBook }, 51.2)]
        [InlineData(new string[] { _firstBook, _secondBook, _thirdBook, _fourthBook, _fifthBook, _firstBook, _secondBook, _thirdBook, _firstBook, _secondBook, _thirdBook }, 72.80)]
        [Theory]
        public void Checkout_WhenMultipleDiscountsArePossible_ReturnsLowestDiscountedPrices(string[] purchasedBooks, decimal expected)
        {
            var bookshop = new Bookshop();

            foreach(string book in purchasedBooks)
            {
                bookshop.AddToBasket(book);
            };

            var checkoutTotal = bookshop.CheckoutBasket();

            Assert.Equal(expected, checkoutTotal.DiscountedPriceTotal);
        }
    }
}
