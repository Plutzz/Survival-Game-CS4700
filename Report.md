## Challenges

- **Inventory Drag and Drop**  
  Implementing a flexible inventory system that supported drag-and-drop interactions was challenging, especially when handling slot swapping, stack splitting, and preventing invalid placements.

- **Crafting Output Logic**  
  Ensuring that crafting recipes produced the correct outputs required careful validation of input items, managing edge cases such as partial ingredients, and preventing item duplication or loss.

- **Multiplayer Netcode**  
  Integrating multiplayer functionality introduced several complexities, particularly around authority and synchronization.
  - **Enemy and Object State Syncing**  
    Keeping enemy health, death states, and world interactions (such as destroyed trees) consistent across all clients required server-authoritative logic and proper use of network variables and RPCs.

## Fixes

- **Inventory System Improvements**  
  The inventory was restructured using a slot-based data model, separating UI logic from inventory data. This made drag-and-drop behavior more predictable and easier to debug.

- **Reliable Crafting Output**  
  Crafting logic was centralized into a single system that validates inputs before consuming items and generating outputs, ensuring consistency and preventing duplication bugs.

- **Server-Authoritative Multiplayer Design**  
  Enemy health and world interactions were handled exclusively on the server using NetworkVariables and ServerRPCs. ClientRPCs were used only for visual feedback, ensuring consistent game state across all players, including late joiners.
