# KARIM BUS SIMULATOR TANZANIA — Graphics Production Plan

## Target visual direction

The project is designed to aim for a realistic African intercity bus-simulator look while remaining optimized for Android. The target is a believable, high-detail West-East African transport environment rather than a cartoon style.

## Quality tiers

- LOW: playable on older Android devices, reduced shadows, reduced texture size, lower vegetation density, lower traffic count.
- MEDIUM: balanced for mainstream tablets and mid-range phones.
- HIGH: recommended for modern mid-range Android devices.
- ULTRA: intended for high-end devices with 6GB+ RAM and strong GPU, using higher shadow distance, stronger materials, and more scene detail.

## Rendering pipeline

- Use URP in Unity 6 for mobile-friendly lighting and optimization.
- Enable baked lighting for static structures where possible.
- Use LOD groups for bus models, buildings, props, trees, and roadside objects.
- Limit real-time shadows to key objects and directional sunlight.
- Use texture compression and atlas packing for Android builds.
- Keep traffic AI and pedestrians limited by quality tier.

## Visual targets

- Realistic bus exteriors and cockpit materials
- Detailed dashboard and passenger cabin
- Road markings, lane lines, and signage
- Tanzanian roadside environments with trees, villages, shops, and buildings
- Weather-driven reflective roads and wet surfaces
- Day/night lighting with realistic bus lights
- Police checkpoints and station lighting

## Mobile optimization guidance

- Prefer baked lighting for non-moving world elements.
- Use occlusion culling in large route scenes.
- Use object pooling for traffic and particle effects.
- Compress textures to ASTC or ETC2 depending on platform support.
- Reduce draw calls by combining static geometry.
- Cap shadow cascade count and resolution on low-end devices.

## Notes

This repo contains the graphics-quality and Android optimization foundation, but actual photorealistic 3D models, textures, and material sets still need to be created or purchased in Unity from real art assets or a content pipeline. The project is structured so those assets can be swapped in without reworking the simulation code.
