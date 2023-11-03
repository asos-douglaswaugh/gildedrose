# Gilded Rose Kata

## The goal of the kata

The goal of this Kata is to practice working with legacy code, and how to effectively understand the code and change it with confidence through a series of incremental operations. In this kata, you'll have the opportunity to experiment with two different techniques to implement give you that confidence, and allow you to implement a new requirement.

### Characterisation Tests

Write some failing tests to try and understand what happens to objects and outputs based on the actions made and the state of the system. Use the output of these tests over time to create groups of tests that define how the system behaves with clear and expressive naming.

These tests will help you to make non-functional changes to the legacy codebase, and give you enough confidence in how the system works to implement the new requirement.

### Golden Master

Use the existing Golden Master test in _ApprovalTest.cs_ to give you confidence as you make refactorings to the code and have confidence that nothing has broken.

This will help you to make changes that are not functional, but make the code clearer and more interpretable.

## The background story

You have just started working at a small shop, and have inherited some code from the previous [MDD developer](https://refuctoring.wordpress.com/2011/01/13/the-mortgage-driven-development-manifesto/).

It is a small inn with a prime location in a prominent city. They buy and sell only the finest goods, but unfortunately, their goods are constantly degrading in quality as they approach their sell by date. They have a system in place that updates the inventory.

- Each item has a `SellIn` value which denotes the number of days we have to sell the item
- Each item has a `Quality` value which denotes how valuable the item is

At the end of each day the system lowers both values for every item. Pretty simple, right? Well this is where it gets interesting:

- Once the sell by date has passed, Quality degrades twice as fast
- The Quality of an item is never negative
- "Aged Brie" actually increases in Quality the older it gets
- The Quality of an item is never more than 50
- "Sulfuras", being a legendary item, never has to be sold or decreases in Quality
- "Backstage passes" have special requirements:
  - Like Aged Brie, "Backstage Passes" increase in Quality as their SellIn value approaches
  - Their Quality increases by 2 when there are 10 days or less and by 3 when there are 5 days or less; but,
  - Their Quality drops to 0 after the concert

## The feature request

You've been asked by the inn to implement a new system requirement:

They have recently signed a supplier of *conjured items*. This requires an update to their system. They want you to change the system so that "Conjured" items degrade in Quality twice as fast as normal items.

Your task is to enable the quality management of *conjured items* to their system, so that they can begin selling a new category of items
