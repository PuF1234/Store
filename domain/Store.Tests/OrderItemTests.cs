﻿using System;
using Xunit;

namespace Store.Tests
{
    public class OrderItemTests
    {
        [Fact]
        public void OrderItem_WithZeroCount_ThrowArgumentOutOfRangeExcection()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                int count = 0;
                new OrderItem(1, 200m, count);
            });
        }
        [Fact]
        public void OrderItem_WithNegativeCount_ThrowArgumentOutOfRangeExcection()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                int count = -1;
                new OrderItem(1, 200m, count);
            });
        }
        [Fact]
        public void OrderItem_WithPositiveCount_SetsCount()
        {
            var orderItem = new OrderItem(1, 3m, 2);

            Assert.Equal(1, orderItem.BicycleId);
            Assert.Equal(2, orderItem.Count);
            Assert.Equal(3m, orderItem.Price);
        }

        [Fact]
        public void Count_WithNegativeNumber_ThrowArgumentOutOfRangeException()
        {
            var orderItem = new OrderItem(0, 0m, 5);

            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                orderItem.Count = -1;
            });

        }

        [Fact]
        public void Count_WithZeroValue_ThrowArgumentOutOfRangeException()
        {
            var orderItem = new OrderItem(0, 0m, 5);

            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                orderItem.Count = 0;
            });

        }

        [Fact]
        public void Count_WithPositiveNumber_SetsValue()
        {
            var orderItem = new OrderItem(0, 0m, 5);

            orderItem.Count = 10;

            Assert.Equal(10, orderItem.Count);
        }
    }
}
