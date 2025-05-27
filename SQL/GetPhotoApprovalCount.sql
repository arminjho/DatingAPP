

CREATE PROCEDURE GetPhotoApprovalCounts()

BEGIN

    select u.UserName AS Username, COUNT(CASE WHEN p.IsApproved = 1 THEN 1 END) AS ApprovedPhotos,
	COUNT(CASE WHEN p.IsApproved = 0 THEN 1 END) AS UnapprovedPhotos

    from photos p

    join aspnetusers u ON u.Id = p.AppUserId

    GROUP by u.UserName;

END 



GRANT EXECUTE ON PROCEDURE GetPhotoApprovalCounts TO 'admin'@'%';

call GetPhotoApprovalCounts();


