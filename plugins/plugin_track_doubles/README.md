# Track doubles

List all tracks occuring more than once.

```sql
SELECT
    *
FROM
    mp3import
WHERE
    (title, artist) IN(
    SELECT
        title,
        artist
    FROM
        mp3import
    GROUP BY
        title,
        artist
    HAVING
        COUNT(title) > 1
    ORDER BY
        title,
        artist
)
ORDER BY
    title,
    artist;
```
