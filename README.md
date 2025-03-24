
## Overview
This project is a modified version of the provided Unity game with updates, new features, and bug fixes.

### Implemented Features
- **Coins & Currency Management**
  - Created a `CurrencyManager` to store and manage currencies. Currencies are saved using ES3 for persistence.
  - *Reference: [CurrencyManager.cs](https://github.com/turanheydarli/Arrow-Wave/blob/main/Assets/Game/Script/Currencies/CurrencyManager.cs)*

- **Store & Skin Customization**
  - Added a Store UI with a dedicated button and integrated skin customization for arrows and bows.
  - Maintained the original UI structure while creating new canvases.
  - *Reference: [ShopScreen.cs](https://github.com/turanheydarli/Arrow-Wave/blob/main/Assets/Game/Script/UI/Screens/ShopPanel/ShopScreen.cs)*

- **Inventory Management**
  - Updated inventory handling for bow and arrow skins.
  - *References: [InventoryManager.cs](https://github.com/turanheydarli/Arrow-Wave/blob/main/Assets/Game/Script/Inventory/InventoryManager.cs), [BowInventoryItem.cs](https://github.com/turanheydarli/Arrow-Wave/blob/main/Assets/Game/Script/Inventory/BowInventoryItem.cs), [ArrowInventoryItem.cs](https://github.com/turanheydarli/Arrow-Wave/blob/main/Assets/Game/Script/Inventory/ArrowInventoryItem.cs)*

### Bug Fixes
- **Arrow Sticking Issue**
  - Fixed the arrow sticking bug by editing `Arrow.cs`.
  - *Reference: [Arrow.cs](https://github.com/turanheydarli/Arrow-Wave/blob/main/Assets/Game/Script/Controllers/Elements/Player/Arrow.cs)*

- **Unlimited Arrows**
  - Added a maximum allowed arrow limit to bow skins to prevent unlimited arrows.

- **Arrow Speed Increase Bug**
  - The arrow speed increase issue was not observed during testing, so no changes were required for this.


