# Artist albums

This plugin will list all albums of an artist.

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
