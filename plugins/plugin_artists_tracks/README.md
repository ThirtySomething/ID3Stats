# Artists tracks

This plugin will list all tracks each artist. First retrieve all artists in alphabetical order.

```sql
SELECT DISTINCT
    artist
FROM
    mp3import
ORDER BY
    artist;
```

Then retrieve for each artist all tracks in alphabetical order:

```sql
SELECT
    *
FROM
    mp3import
WHERE
    artist = <Artist from above>
ORDER BY
    artist;
```
