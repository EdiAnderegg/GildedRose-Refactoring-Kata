using System;
using System.Collections.Generic;

namespace GildedRoseKata;


/*
 * 
 * 
 * 1. Understand the Rules
 * What is really needed ?
 * 
 * 2. Understand the Code
 * In order to understand the code and see if everything works
 * as expected my approach will be, Write test first that test the logic of the Business Rules. 
 * 
 * So my approach was write the business rules like a "checklist" in a paper go and check one rule
 * at a time with a test and repeat. 
 * 
 * This wil help alot to understand what is actually happening in the Code
 * and also I will be able to find Bugs in the System, if any.
 * 
 * 
 * 3. Now that I have all tests Cases that cover the needed Business Rules,
 * 
 *    - First before changing the messi code I will try to add conjure items logic and make the test pass.
 *    
 *    - Then i will make small changes to the if statments, like try to separate the 
 *    if statements to own Items Categories
 * 
 * 
 * 
 */

public static class ItemCategory
{
    public static readonly string Normal = "";
    public static readonly string Sulfuras = "Sulfuras, Hand of Ragnaros";
    public static readonly string AgedBrie = "Aged Brie";
    public static readonly string BackstagePasses = "Backstage passes to a TAFKAL80ETC concert";
    public static readonly string Conjured = "Conjured Mana Cake";
}

public class GildedRose
{
    IList<Item> Items;

    public GildedRose(IList<Item> Items)
    {
        this.Items = Items;
    }


    public Item GetItemByIndex(int index)
    {
        return Items[index];
    }

    public void UpdateQuality()
    {

        for (var i = 0; i < Items.Count; i++)
        {


            // Item Properties
            string itemName = Items[i].Name;
            int itemSellIn = Items[i].SellIn;
            int itemQuality = Items[i].Quality;

            // Item Rules
            int MinSellInValue = 0;
            int MinQualityValue = 0;
            int MaxQualityValue = 50;


            // Category: Conjured Items
            if (itemName == ItemCategory.Conjured)
            {
                // Check Rules
                if (itemQuality > MaxQualityValue) Items[i].Quality = MaxQualityValue;

                //SellIn Rules
                Items[i].SellIn = itemSellIn - 1;

                // Quality Rules
                // Conjured Items Degrade twice as fast as Normal Items
                if (itemQuality <= MinQualityValue) Items[i].Quality = MinQualityValue;
                else Items[i].Quality = itemQuality - 2;

                break;
            }

            if (Items[i].Name != "Aged Brie" && Items[i].Name != "Backstage passes to a TAFKAL80ETC concert")
            {
                if (Items[i].Quality > 0)
                {
                    if (Items[i].Name != "Sulfuras, Hand of Ragnaros")
                    {
                        Items[i].Quality = Items[i].Quality - 1;
                    }
                }
            }
            else
            {
                if (Items[i].Quality < 50)
                {
                    Items[i].Quality = Items[i].Quality + 1;

                    if (Items[i].Name == "Backstage passes to a TAFKAL80ETC concert")
                    {
                        if (Items[i].SellIn < 11)
                        {
                            if (Items[i].Quality < 50)
                            {
                                Items[i].Quality = Items[i].Quality + 1;
                            }
                        }

                        if (Items[i].SellIn < 6)
                        {
                            if (Items[i].Quality < 50)
                            {
                                Items[i].Quality = Items[i].Quality + 1;
                            }
                        }
                    }
                }
            }



            if (Items[i].Name != "Sulfuras, Hand of Ragnaros")
            {
                Items[i].SellIn = Items[i].SellIn - 1;
            }

            if (Items[i].SellIn < 0)
            {
                if (Items[i].Name != "Aged Brie")
                {
                    if (Items[i].Name != "Backstage passes to a TAFKAL80ETC concert")
                    {
                        if (Items[i].Quality > 0)
                        {
                            if (Items[i].Name != "Sulfuras, Hand of Ragnaros")
                            {
                                Items[i].Quality = Items[i].Quality - 1;
                            }
                        }
                    }
                    else
                    {
                        Items[i].Quality = Items[i].Quality - Items[i].Quality;
                    }
                }
                else
                {
                    if (Items[i].Quality < 50)
                    {
                        Items[i].Quality = Items[i].Quality + 1;
                    }
                }
            }
        }
    }
}