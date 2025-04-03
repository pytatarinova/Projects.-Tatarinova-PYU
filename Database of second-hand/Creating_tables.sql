create table Posts (
  post varchar(30) primary key, 
  salary numeric(8, 2) not null, 
  constraint check_salary check(salary >= 19000)
);
-- 19000 – размер МРОТ на момент создания таблицы.


create table Education (
  education_id numeric(1) primary key, 
  education_name varchar(20) not null
);


create table Colors (
  color_id numeric(3) primary key, 
  color varchar(30) not null
);


create table Categories (
  category_id numeric(3) primary key, 
  category varchar(50) not null
);


create table Materials (
  material_id numeric(3) primary key, 
  material varchar(30) not null
);

create table Brands (
  brand_id numeric(4) primary key, 
  brand varchar(30) not null
);

 
create table Staff (
  staff_surname varchar(25) not null, 
  staff_name varchar(30) not null, 
  staff_born date not null, 
  staff_sex char(1) not null constraint check_sex check(
    staff_sex in ('ж', 'м')), 
  staff_passport char(10) not null constraint uniq_pasp unique constraint check_pasp check(
    cast(staff_passport as int)> 100000000), 
  staff_inn char(12) primary key constraint uniq_inn unique constraint check_inn check(
    cast(staff_inn as int)> 10000000000), 
  staff_snils char(11) not null constraint uniq_snils unique constraint check_snils check(
    cast(staff_snils as int)> 1000000000), 
  staff_post varchar(30) not null constraint fk_staff_posts references Posts, 
  staff_education numeric(1) not null constraint fk_edu_posts references Education, 
  staff_chief char(12) references Staff
);


create table Staff_phem (
  staff_id char(12) not null constraint fk_phem_staff references Staff, 
  staff_phone varchar(20), 
  staff_email varchar(20), 
  constraint check_phem check(
    staff_phone is not null 
    or staff_email is not null)
);


create table Staff_adresses (
  staff_id char(12) not null constraint fk_phem_staff references Staff, 
  staff_country varchar(20) not null, 
  staff_region varchar(20) not null, 
  staff_city varchar(20) not null, 
  staff_street varchar(20) not null, 
  staff_house varchar(5) not null, 
  staff_letter char(1), 
  staff_flat varchar(5)
);


create table Suppliers (
  supplier_name varchar(50) not null, 
  supplier_type varchar(12) not null constraint check_type check(
    supplier_type in ('фирма', 'частное лицо')), 
  supplier_inn varchar(12) primary key constraint uniq_suppl_inn unique constraint check_inn check(
    cast(supplier_inn as int)> 100000000), 
  supplier_manager_surname varchar(25) not null, 
  supplier_manager_name varchar(30) not null, 
  supplier_conditions varchar(100), 
  supplier_start_date date not null, 
  supplier_end_date date constraint check_end_date check(
    supplier_end_date > supplier_start_date), 
  supplier_staff char(12) not null constraint fk_supplier_staff references Staff
);


create table Suppliers_phem (
  supplier_id char(12) not null constraint fk_phem_supplier references Suppliers, 
  supplier_phone varchar(20), 
  supplier_email varchar(20), 
  constraint check_phem check(
    supplier_phone is not null 
    or supplier_email is not null)
);


create table Suppliers_adresses (
  supplier_id char(12) not null constraint fk_phem_supplier references Suppliers, 
  supplier_country varchar(20) not null, 
  supplier_region varchar(20) not null, 
  supplier_city varchar(20) not null, 
  supplier_street varchar(20) not null, 
  supplier_house varchar(5) not null, 
  supplier_letter char(1)
);


create table Buyers (
  buyer_id char(10) primary key, 
  buyer_surname varchar(25), 
  buyer_name varchar(30), 
  buyer_staff char(12) not null constraint fk_buyer_staff references Staff
);


create table Buyers_phem (
  buyer_id char(10) not null constraint fk_phem_buyer references Buyers, 
  buyer_phone varchar(20), 
  buyer_email varchar(20), 
  constraint check_phem check(
    buyer_phone is not null 
    or buyer_email is not null)
);


create table Buyers_adresses (
  buyer_id char(10) not null constraint fk_phem_staff references Buyers, 
  buyer_country varchar(20) not null, 
  buyer_region varchar(20) not null, 
  buyer_city varchar(20) not null, 
  buyer_street varchar(20) not null, 
  buyer_house varchar(5) not null, 
  buyer_letter char(1), 
  buyer_flat varchar(5)
);


create table Products (
  product_id char(10) primary key, 
  product_category numeric(3) not null constraint fk_product_category references Categories, 
  product_sign char(1) not null constraint check_sign check(
    product_sign in ('1', '0')), 
  product_price numeric(8, 2) not null constraint check_price check(product_price > 0), 
  product_size varchar(5) not null, 
  product_sex char(1) not null constraint check_sex check(
    product_sex in ('м', 'ж', 'у')), 
  product_brand numeric(4) references Brands, 
  product_material numeric(3) references Materials, 
  product_color numeric(3) not null constraint fk_product_color references Colors, 
  product_state numeric(1) not null constraint check_state check(
    product_state >= 1 
    AND product_state <= 5), 
  product_features varchar(100), 
  product_buyer char(10) references Buyers, 
  product_supplier varchar(12) not null constraint fk_product_supplier references Suppliers, 
  product_staff char(12) references Staff
);
