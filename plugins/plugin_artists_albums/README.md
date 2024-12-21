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

## Sample output

An example of the output can be viewed [here][app_statistic].

[app_statistic]: ./../../sample/All%20albums%20per%20artist.html
