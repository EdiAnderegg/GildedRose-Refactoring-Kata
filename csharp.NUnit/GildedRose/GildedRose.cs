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
 *    - First before changing the messy code I will try to add conjure items logic and make the test pass.
 *    
 *    - Then I will make small changes to the if statments, like try to separate the 
 *    if statements to own Items Categories this will allow me to make each logic dependent and 
 *    a bit more readable. 
 *    
 * 4. Now its time to maybe create an ItemObject with category methods or Objects that inherit form Item 
      each Item category with its own rules
 * in GRASP Principle, this would be Information Expert, add the rules to the Object
 * of the same context or Dependency Injection hmmmmmmmmmmmmmm
 * 
 * Ok Dependency Injection would be here a good Option. I will go for dependency Injection
 * 
 * 
 * 
 */



// Category Classes
public static class ItemCategory
{
    public static readonly string Sulfuras = "Sulfuras, Hand of Ragnaros";
    public static readonly string AgedBrie = "Aged Brie";
    public static readonly string BackstagePasses = "Backstage passes to a TAFKAL80ETC concert";
    public static readonly string Conjured = "Conjured Mana Cake";

}

public interface IItemUpdateRule
{
    public bool AppliesTo(string itemName);
    public void Update(Item item);
}

public class NormalUpdateRule : IItemUpdateRule
{
    private int _MinQualityValue = 0;
    private int _MaxQualityValue = 50;
    private int _SellInDay = 0;

    public bool AppliesTo(string itemName)
    {
        return true;
    }

    public void Update(Item item)
    {

        //SellIn Rules
        item.SellIn = item.SellIn - 1;

        // Quality Rules
        if (item.Quality <= _MinQualityValue) item.Quality = _MinQualityValue;

        else if (item.Quality < _MaxQualityValue)
        {
            if (item.SellIn < _SellInDay) item.Quality = item.Quality - 2;
            else item.Quality = item.Quality - 1;
        }

        if (item.Quality >= _MaxQualityValue) item.Quality = _MaxQualityValue;

    }
}


public class GildedRose
{
    IList<Item> Items;
    IList<IItemUpdateRule> Rules;

    public GildedRose(IList<Item> Items, IList<IItemUpdateRule> Rules)
    {
        this.Items = Items;
        this.Rules = Rules;
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
                //SellIn Rules
                Items[i].SellIn = itemSellIn - 1;

                // Quality Rules
                // Conjured Items Degrade twice as fast as Normal Items
                if (itemQuality <= MinQualityValue) Items[i].Quality = MinQualityValue;
                else Items[i].Quality = itemQuality - 2;

                if (Items[i].Quality >= MaxQualityValue) Items[i].Quality = MaxQualityValue;
                
                continue;
            }

            // Category: Aged Brie

            if (itemName == ItemCategory.AgedBrie)
            {
                //SellIn Rules
                Items[i].SellIn = itemSellIn - 1;

                // Quality Rules

                if (itemQuality <= MinQualityValue) Items[i].Quality = MinQualityValue;
                
                if(itemSellIn <= 0) Items[i].Quality = itemQuality + 2;
                else Items[i].Quality = itemQuality + 1;

                if (Items[i].Quality >= MaxQualityValue) Items[i].Quality = MaxQualityValue;

                continue;
            }

            // Category: Sulfuras

            if (itemName == ItemCategory.Sulfuras)
            {
                // Quality Rules
                Items[i].Quality = 80;
                
                continue;
            }

            // Category: Backstage passes

            if (itemName == ItemCategory.BackstagePasses)
            {

                //SellIn Rules
                Items[i].SellIn = itemSellIn - 1;

                // Quality Rules

                if (itemQuality <= MinQualityValue) Items[i].Quality = MinQualityValue;

                if (itemSellIn <= MinSellInValue) Items[i].Quality = MinQualityValue;
                else if (itemQuality < MaxQualityValue)
                {
                    if (itemSellIn <= 5) Items[i].Quality = itemQuality + 3;
                    else if (itemSellIn <= 10) Items[i].Quality = itemQuality + 2;
                    else Items[i].Quality = itemQuality + 1;
                }

                if (Items[i].Quality > MaxQualityValue) Items[i].Quality = MaxQualityValue;

                continue;
            }

            foreach (IItemUpdateRule itemRule in Rules)
            {
                if (itemRule.AppliesTo(Items[i].Name)) itemRule.Update(Items[i]);
            }

        }
    }
}