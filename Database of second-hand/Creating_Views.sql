CREATE VIEW Contact_Details AS
SELECT s.staff_inn AS "staff_INN",
       s.staff_surname || ' ' || s.staff_name AS "staff_full_name",
       sp.staff_phone AS "staff_phone",
       sp.staff_email AS "staff_email"
FROM Staff s
LEFT JOIN Staff_phem sp ON s.staff_inn = sp.staff_id;


CREATE VIEW Salary_Details AS
SELECT s.staff_inn AS "staff_INN",
       s.staff_surname || ' ' || s.staff_name AS "staff_full_name",
       s.staff_post AS "staff_post",
       p.salary AS "staff_salary"
FROM Staff s
JOIN Posts p ON s.staff_post = p.post;


CREATE VIEW Staff_Buyers AS
SELECT s.staff_inn AS "staff_INN",
       s.staff_surname || ' ' || s.staff_name AS "staff_full_name",
       (SELECT string_agg(b.buyer_id, ', ')
        FROM Buyers b
        WHERE b.buyer_staff = s.staff_inn) AS "staff_buyers"
FROM Staff s;


CREATE VIEW Active_Suppliers AS
SELECT s.supplier_name AS "supplier_name",
       s.supplier_inn AS "supplier_inn",
       sa.supplier_country || ', ' || sa.supplier_region || ', ' || sa.supplier_city || ', ' || sa.supplier_street || ' ' || sa.supplier_house || COALESCE(' ' || sa.supplier_letter, '') AS "supplier_adress",
       sp.supplier_phone AS "supplier_phone",
       sp.supplier_email AS "supplier_email"
FROM Suppliers s
JOIN Suppliers_phem sp ON s.supplier_inn = sp.supplier_id
JOIN Suppliers_adresses sa ON s.supplier_inn = sa.supplier_id
WHERE s.supplier_end_date IS NULL;


CREATE VIEW Staff_Supplier_Contracts AS
SELECT s.staff_inn AS "staff_inn",
       s.staff_surname || ' ' || s.staff_name AS "staff_full_name",
       string_agg(sp.supplier_inn, ', ') AS "staff_suppliers"
FROM Staff s
JOIN Suppliers sp ON s.staff_inn = sp.supplier_staff
GROUP BY s.staff_inn, s.staff_surname, s.staff_name;


CREATE VIEW Available_Products AS
SELECT p.product_id,
       p.product_price,
       p.product_size,
       p.product_sex,
       p.product_features,
       b.brand AS "product_brand",
       m.material AS "product_material",
       c.category AS "product_category",
       co.color AS "product_color",
       p.product_state
FROM Products p
JOIN Brands b ON p.product_brand = b.brand_id
JOIN Materials m ON p.product_material = m.material_id
JOIN Categories c ON p.product_category = c.category_id
JOIN Colors co ON p.product_color = co.color_id
WHERE p.product_sign = '1';
