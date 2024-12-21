# Tracks artist

Number of total tracks for each artist:

```sql
SELECT
    artist,
    COUNT(artist) AS tracks_total
FROM
    mp3import
GROUP BY
    artist
ORDER BY
    tracks_total DESC,
    artist;
```

## Sample output

An example of the output can be viewed [here][app_statistic].

[app_statistic]: ./../../sample/Total%20tracks%20per%20artist.html
