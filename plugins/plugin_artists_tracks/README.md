# Artists tracks

This plugin will list all tracks each artist. First retrieve all artists in alphabetical order.

```sql
SELECT
    artist,
    title
FROM
    mp3import
ORDER BY
    artist,
    title,
    id;
```
