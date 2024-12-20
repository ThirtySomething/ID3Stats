# Artist albums

This plugin will list all albums of an artist. First retrieve all artists in alphabetical order.

```sql
SELECT DISTINCT
    artist,
    album
FROM
    mp3import
GROUP BY
    artist,
    album
ORDER BY
    artist,
    album;
```
