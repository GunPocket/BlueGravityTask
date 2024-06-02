   # Game Shop System

   This repository contains the code for a basic game shop system implemented in Unity. The system allows players to purchase items using in-game currency, manage their inventory, and equip clothing items. This task was assigned by Blue Gravity Studio.

   ## Scripts Overview
   ### PlayerController.cs

       - Manages the player's money and updates the UI accordingly.
       - References the InventoryManager for item management.

   ### InventoryManager.cs

       - Manages the player's inventory.
       - Allows adding and equipping items.

   ### ClothingItem.cs

       - Represents an item of clothing that can be purchased and equipped.
       - Contains attributes like item name, price, and sprite.

   ### ShopItemUI.cs

       - Manages the shop UI for individual items.
       - Handles item purchase logic.

   ### WardrobeUI.cs

       - Manages the wardrobe UI for displaying equipped items.
       - Handles the logic for equipping items from the inventory.

   ### EquipClothes.cs

       - Equips clothing items on the player character.
       - Updates the visual representation of the player based on equipped items.

   # How to Use

   1. Ensure all scripts are attached to the correct GameObjects in the Unity Editor.
   2. Set up the UI elements for the shop and wardrobe interfaces.
   3. Create ClothingItem assets in the Unity Editor and assign their attributes.
   4. Run the game and use the shop to purchase and equip items.

   # License

   This project is licensed under the MIT License - see the LICENSE file for details.
