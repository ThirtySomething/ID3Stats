# Duration collection

Statistics about the duration over the complete collection:

- Shortest track duration as well as track and interpret
- Average track duration as well as the total amount of tracks
- Longest track duration as well as track and interpret
- Duration over all tracks

```sql
SELECT
    'track_count_total',
    COUNT(id)
FROM
    mp3import
UNION ALL
SELECT
    'track_duration_min',
    MIN(durationms)
FROM
    mp3import
UNION ALL
SELECT
    'track_duration_average',
    AVG(durationms)
FROM
    mp3import
UNION ALL
SELECT
    'track_duration_max',
    MAX(durationms)
FROM
    mp3import
UNION ALL
SELECT
    'track_duration_total',
    SUM(durationms)
FROM
    mp3import;
```
