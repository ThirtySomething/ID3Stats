# Duration collection

Statistics about the duration over the complete collection:

- Shortest track duration as well as track and interpret
- Average track duration as well as the total amount of tracks
- Longest track duration as well as track and interpret
- Duration over all tracks

```sql
SELECT
    COUNT(id) AS track_count_total,
    MIN(durationms) AS track_duration_min,
    AVG(durationms) AS track_duration_average,
    MAX(durationms) AS track_duration_max,
    SUM(durationms) AS track_duration_total
FROM
    mp3import;
```
