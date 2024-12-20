# Artists tracks

This plugin will list all tracks each artist.

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
