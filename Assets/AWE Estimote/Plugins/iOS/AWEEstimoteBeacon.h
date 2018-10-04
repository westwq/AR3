#import <Foundation/Foundation.h>
#import <EstimoteSDK/EstimoteSDK.h>

@interface AWEEstimoteBeacon : NSObject

@property NSString* beaconName;
@property NSString* identifier;
@property NSUUID* proximityUUID;
@property NSString* macAddress;
@property NSNumber* major;
@property NSNumber* minor;
@property NSInteger rssi;
@property NSInteger measuredPower;
@property CLProximity proximity;
@property CLLocationAccuracy accuracy;
@end