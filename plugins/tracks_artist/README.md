# Tracks artist

Number of tracks for each artist:

```sql
SELECT
    artist,
    COUNT(artist) AS tracks_total
FROM
    mp3import
GROUP BY
    artist
ORDER BY
    tracks DESC,
    artist;
```
