create index ind_sup_staff on Suppliers(supplier_staff);
create index ind_buyer_staff on Buyers(buyer_staff);
create index ind_prod_categ on Products(product_category);
create index ind_sup_phem_id on Suppliers_phem(supplier_id);

create index ind_buyer_name on Buyers(buyer_surname, buyer_name);
create index ind_sup_man_name on Suppliers(supplier_manager_surname, supplier_manager_name);
create index ind_sup_name_date on Suppliers(supplier_name, supplier_name);
