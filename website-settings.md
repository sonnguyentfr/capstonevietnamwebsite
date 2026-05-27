# Website Settings Rules

Current source:

PortalController.GetPortalSetting(settingKey, portalId, defaultValue)

Existing code has many duplicated calls.

Requirements:

1. Create strongly typed models:
- SiteSettings
- BranchInfo
- SocialInfo
- ChatInfo

2. Create service:
- ISiteSettingsHelper
- SiteSettingsHelper

3. Use:
- Dependency Injection
- IMemoryCache
- Async methods

Method:

Task<SiteSettings> GetSettingsAsync(int portalId)

Rules:

- Do not duplicate GetPortalSetting
- Support multiple portals
- Null safe
- Production ready
- Reusable across entire website
- Load all settings once
- Cache result by portalId

Usage:

Inject ISiteSettingsHelper into Blazor pages/components

Example:

@inject ISiteSettingsHelper SiteHelper

var site = await SiteHelper.GetSettingsAsync(portalId);