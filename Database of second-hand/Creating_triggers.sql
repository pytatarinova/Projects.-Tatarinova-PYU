CREATE OR REPLACE FUNCTION check_staff_post()
RETURNS TRIGGER AS $$
BEGIN
    IF EXISTS (
        SELECT 1 
        FROM Staff 
        WHERE staff_inn = NEW.buyer_staff 
        AND staff_post IN ('Администратор', 'Грузчик', 'Уборщик')
    ) THEN
        RAISE EXCEPTION 'Сотрудник с должностью "%", "%", или "%" не может быть зарегистрирован как покупатель', 'Администратор', 'Грузчик', 'Уборщик';
    END IF;
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;


CREATE TRIGGER prevent_staff_post_buyer
BEFORE INSERT ON Buyers
FOR EACH ROW
EXECUTE FUNCTION check_staff_post();


CREATE OR REPLACE FUNCTION check_staff_post_supplier()
RETURNS TRIGGER AS $$
BEGIN
    IF EXISTS (
        SELECT 1 
        FROM Staff 
        WHERE staff_inn = NEW.supplier_staff 
        AND staff_post IN ('Консультант-кассир', 'Грузчик', 'Уборщик')
    ) THEN
        RAISE EXCEPTION 'Сотрудник с должностью "%", "%", или "%" не может заключать договоры с поставщиками', 'Консультант-кассир', 'Грузчик', 'Уборщик';
    END IF;
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER prevent_staff_post_supplier
BEFORE INSERT ON Suppliers
FOR EACH ROW
EXECUTE FUNCTION check_staff_post_supplier();
