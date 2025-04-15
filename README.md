# Inventory Manager API

## Project Outline

#### 1. Reconfigure API that can be used to manage the inventory of a small shop.

<hr>

#### 2. Controllers
1. Add InventoryController
2. Route: /items

<hr>

#### 3. Services

1. Create IItemService + ItemService:
2. Inject ItemService into Controller

<hr>

#### 4. API Endpoints (CRUD)
1. GET /items – List all items.
2. GET /items/{id} – Get single item by ID.
3. POST /items – Create new item.
4. PUT /items/{id} – Update existing item.
5. DELETE /items/{id} – Delete item.
6. Map endpoints to service methods.

<hr>

#### 5. Business Logic - Discounts

1. Add service method to calculate best discount:
    - Monday 12–5pm: 50%
    - Stock > 10: 20%
    - Stock > 5: 10%

2. Only apply max applicable discount.
3. Handle discount logic centrally

<hr>

#### 6. Inject discount logic in:

1. GET all items
2. GET single item

<hr>

#### Testing to include
1. Unit test for discount logic
    - Edge cases
    - Time-based logic
2. Unit tests for service methods***

### Future Possible Enhancements (Not Implemented Yet)
- ***Integration Tests
- Pagination Support to GET all items and GET itemById (for variations if needed)
