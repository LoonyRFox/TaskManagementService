CREATE TABLE client_payments
(
    id BIGSERIAL PRIMARY KEY,
    client_id BIGINT NOT NULL,
    dt TIMESTAMP NOT NULL,
    amount NUMERIC(12,2) NOT NULL
);

INSERT INTO client_payments (client_id, dt, amount)
VALUES
(1, '2022-01-03 17:24:00', 100),
(1, '2022-01-05 17:24:14', 200),
(1, '2022-01-05 18:23:34', 250),
(1, '2022-01-07 10:12:38', 50),
(2, '2022-01-05 17:24:14', 278),
(2, '2022-01-10 12:39:29', 300);

CREATE OR REPLACE FUNCTION get_daily_payments(
    p_client_id BIGINT,
    p_date_from DATE,
    p_date_to DATE
)
RETURNS TABLE(payment_date DATE, total_amount NUMERIC) AS
$$
BEGIN
    RETURN QUERY
    SELECT d::date AS payment_date,
           COALESCE(SUM(cp.amount), 0) AS total_amount
    FROM generate_series(p_date_from, p_date_to, interval '1 day') d
    LEFT JOIN client_payments cp
           ON cp.client_id = p_client_id
          AND cp.dt::date = d::date
    GROUP BY d
    ORDER BY d;
END;
$$ LANGUAGE plpgsql;
