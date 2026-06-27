using System.Collections.Generic;
using GildedRoseKata;
using NUnit.Framework;

namespace GildedRoseTests;

public class GildedRoseTest
{


    // TestCases of Category: Normal
    [TestCase("+5 Dexterity Vest", 10 ,10, 9)]

    // TestCases of Category: Aged Brie
    [TestCase("Aged Brie", 10, 8, 9)]

    // TestCases of Category: Sulfuras
    [TestCase("Sulfuras, Hand of Ragnaros", 0, 80, 0)]
    [TestCase("Sulfuras, Hand of Ragnaros", -1, 80, -1)]
    public void UpdateQuality_Should_Update_Item_SellIn(
        string name,
        int sellIn,
        int quality,
        int expectedSellIn)
    {
        // Arrange
        var item = new Item
        {
            Name = name,
            SellIn = sellIn,
            Quality = quality
        };

        var items = new List<Item> { item };
        var updateRules = new List<IItemUpdateRule>
        {
            new AgedBrieUpdateRule(),
            new ConjuredUpdateRule(),
            new NormalUpdateRule()
        };

        var app = new GildedRose(items, updateRules);

        // Act
        app.UpdateQuality();

        // Assert
        Assert.That(item.SellIn, Is.EqualTo(expectedSellIn));
    }

    // TestCases of Category: Normal
    [TestCase("+5 Dexterity Vest", 10, 8, 7)]
    [TestCase("+5 Dexterity Vest", 0, 8, 6)]
    [TestCase("+5 Dexterity Vest", 10, 0, 0)]
    [TestCase("+5 Dexterity Vest", 10, 51, 50)]

    // TestCases of Category: Aged Brie
    [TestCase("Aged Brie", 10, 8, 9)]
    [TestCase("Aged Brie", 10, 50, 50)]

    // TestCases of Category: Sulfuras
    [TestCase("Sulfuras, Hand of Ragnaros", 0, 80, 80)]
    [TestCase("Sulfuras, Hand of Ragnaros", -1, 80, 80)]

    // TestCases of Category: Backstage passes
    [TestCase("Backstage passes to a TAFKAL80ETC concert", 10, 20, 22)]
    [TestCase("Backstage passes to a TAFKAL80ETC concert", 5, 20, 23)]
    [TestCase("Backstage passes to a TAFKAL80ETC concert", 0, 20, 0)]

    // TestCases of Category: Conjured
    [TestCase("Conjured Mana Cake", 10, 20, 18)]

    public void UpdateQuality_Should_Update_Item_Quality(
    string name,
    int sellIn,
    int quality,
    int expectedQuality)
    {
        // Arrange
        var item = new Item
        {
            Name = name,
            SellIn = sellIn,
            Quality = quality
        };

        var items = new List<Item> { item };
        var updateRules = new List<IItemUpdateRule>
        {
            new AgedBrieUpdateRule(),
            new ConjuredUpdateRule(),
            new NormalUpdateRule()
        };

        var app = new GildedRose(items, updateRules);

        // Act
        app.UpdateQuality();

        // Assert
        Assert.That(item.Quality, Is.EqualTo(expectedQuality));
    }

}